## Uutiset ##
* **Wiki on julkaistu! (14.3.2020)**

---

# Suunnitteludokumentti #

## Ohjelman rakenne & komponentit ##
Toteutus koostuu pääasiassa kolmen pääkomponentin kokonaisuutena: Client, Services ja API(t). Jokainen edellä mainituista komponenteista toteutetaan omaan projektiinsa. Näiden lisäksi omissa projekteissaan toteutetaan ydintason toteutuksia, jotka sisältävät projekteille yhteistä toiminnallisuutta. Ohjelman kokonaistoteutus pyrkii hallitsemaan riippuvuuksia sipuliarkkitehtuurin mukaisesti.


![picture alt](https://i.imgur.com/bnjs88p.png)
*Havainnekuva ohjelman kokonaisarkkitehtuurista*


Kokonaisuutena riippuvuuksia hallitaan IoC-containerilla (Unity), johon tarvittavat riippuvuudet rekisteröidään juuriprojektissa. Avalonia-frameworkin rajoitteiden vuoksi osa vain UI:n tarvitsemista rekisteröinneistä suoritetaan kuitenkin Client-projektissa.

### Client ###
Client toteuttaa graafisen käyttöliittymän ja rakentuu Avalonia-, ReactiveUI- ja Prism-frameworkien päälle. Käyttöliittymän arkkitehtuuri toteuttaa MVVM-patternia: Jokaista näkymää (view) kohden on olemassa näkymämalli (view model), jonka näkymä omistaa ja josta näkymä hakee tarvitsemansa tiedot näytettäväkseen. Tarvittaessa näkymämalli käyttää mallia (model), jonka se omistaa. Lopputuloksena näkymän "code-behindissa" ei ole alustusta suurempaa toiminnallisuutta, eikä näkymämallilla suoriteta business-logiikkaa.

### Services ###
Services-projekti pyrimme sisältää suurimman osan sovelluksen liiketoimintalogiikasta. Se kommunikoi ulkoisten rajapintojen kanssa tarvitessaan ulkoista dataa – esimerkiksi Finlandia-hiihdon tulosarkistosta. Services-puolelta haettu ja käsitelty data lähetetään Client-puolelle, josta se MVVM-patternin mukaisesti siirtyy käyttöliittymään joko suoraan tai mahdollisimman kevyin muokkauksin.

### API ###
API puoli hoitaa datan hakemisen datalähteestä (aluksi vain Finlandia-hiihdon dataa hakeva API on toteutettu). API määrittelee funktion ja hyväksymänsä hakuehdot, joilla ulkoisesta lähteestä peräisin olevaa dataa voidaan hakea koneluettavassa muodossa. Eri API-toteutuksissa voidaan määritellä useita rajapintoja ja datan hakijoita eri datalähteille. API voi sisältää toteutuksen siitä millä tavoin dataa haetaan vai tallenetaanko se nopeammin saataville esimerkiksi erilaisia cacheja hyödyntäen. API:n mahdolliset cache-toteutukset ja vastaavat eivät kuitenkaan näy sen itsensä ulkopuolelle.

### Kehittämisen suunnitelma ###
Pyrkimyksenä on mahdollistaa uusien ominaisuuksien kehittäminen "viipaleina". Käytännössä tämä tarkoittaa esimerkiksi yksittäisen uuden näkymän kehittämisessä mallia, jossa kehittäjä luo Client-projektiin uuden näkymän, näkymämallin ja mallin ja määrittelee tarvittaessa uuden rajapinnan ServiceInterfaces-projektiin mahdollistakseen datan hakemisen ja käsittelyn Clientin ulkopuolella siten, että Clientille kirjoitettava business-logiikka jää minimaaliseksi. Mikäli kehittäjä lisää ServiceInterfaces-projektiin uuden rajapinnan, tekee hän kyseisen rajapinnan toteutuksen Services-projektiin ja rekisteröi rajapinnan ja sen toteutuksen juuriprojektissa; uuteen näkymämalliin injektoidaan kyseinen rajapinta rakentajan kautta käytettäväksi. Uusi näkymä muotoillaan näyttämään data halutulla tavalla. Lopuksi käyttäjä lisää haluamaansa paikksan (jokin toinen ohjelman näkymä) navigoinnin uuteen näkymään, jonka jälkeen toteutettu uutuus on käytössä myös käyttöliittymältä.


![picture alt](https://jimmybogardsblog.blob.core.windows.net/jimmybogardsblog/3/2018/Picture0030.png)

*Periaatekuva viipaleesta ([Jimmy Bogard: Vertical Slice Architecture](https://jimmybogard.com/vertical-slice-architecture/), viitattu 14.3.2020)*


Ohjelman kehittämisessä viipaleina on lukuisia etuja:

* Koko tiimin työskenteleminen omien, erillisten ominaisuuksien kehittämisen kanssa on helpompaa.
* Kaikki tiimin jäsenet oppivat kehittämään jokaista kokonaisuuden osa-aluetta.
* Versionhallinnan konflikteja tulee vähemmän ja ne ovat usein helpompia selvittää.
* Päällekkäisen työn riski pienenee.

***Ohjelman arkkitehtuuri ei kuitenkaan pyri ”viipalearkkitehtuuriin” sen täydessä laajuudessa.***

## Rajapintadokumentaatio ##

### FinlandiaHiihtoAPI ###

#### Enumit ####

```csharp
public enum Gender
{
	Male, Female
}

public enum competitionType 
{
	P20, V20, V20jun, P25, P30, V30, P32, V32, P35, P42, V42, P44,
	P45, V45, P50, V50, P52, P53, V53, P60, P62, P75, V75, P100
}
public enum ageGroup
{
	U35, A35, A40, A45, A50, A55, A60, A65, A70, A75, O80
}
```

#### Virheet ####
Näihin voi vielä tulla muutoksia ja lisäyksiä, esim. jos tallennetaan dataa lokaalisti, niin datan haku suoraan sivustolta ei vielä täysin välttämätöntä.

* **`CantAccessSiteException`** : Sivustoon ei saatu yhteyttä (esim. ei Internet-yhteyttä tai Finlandia-hiihto sivusto on alhaalla.)
* **`InvalidArgumentsException`** : Sivusto palautti virheen viallisten argumenttien takia
* **`TooMuchDataException`** : Yli 10000 tulosta haulla, jolloin sivusto ei palauta dataa.

#### Funktiot
```csharp
public async Task<IEnumerable<Dictionary<string, string>>> GetData(...)
```
### Parameterit ###
Parametreilla määritetään filttereita datan haulle.
Luomme myöhemmin args-luokan, jolla filttereiden asetus siistimpää.

* **`year`**:  **Type**: int, **Default**: null
*  **`firstName`**:  **Type**: string,  **Default**: null
* **`lastName`**: **Type**: string, **Default**: null
* **`competitionType`**: **Type**: enum, **Default**: null
* **`ageGroup`**: **Type**: enum, **Default**: null
*  **`competitorHomeTown`**: **Type**: string, **Default**: null
*  **`team`** : **Type**: string, **Default**: null
* **`gender`**: **Type**: enum, **Default**: null
* **`nationality`**: **Type**: string, **Default**: null

### Palauttaa ###
```csharp
<IEnumerable<Dictionary<string, string>>>
```

Palauttaa asynkronisesti iteroitavan joukon Dictionary-tietueita, joissa yksi tietue edustaa yhtä tulosriviä. Dictionaryn avaimet ovat tulosrivin sarakkeen nimiä ja arvot vastaavat solun arvoa kyseisessä sarakkeessa.

**Dictionaryn avaimet:**

* ”Vuosi"
* ”Matka"
* "Tulos"
* "Sija"
* "Sija/Miehet"
* "Sija/Naiset"
* "Sukupuoli"
* "Sukunimi Etunimi"
* "Paikkakunta"
* "Kansallisuus"
* "Syntymävuosi"
* "Joukkue"
