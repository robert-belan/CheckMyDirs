# CheckMyDirs v0.2.0b
_Super Primitive Version Control System_

Tento miniprojekt je výsledkem zadání drobného úkolu, jehož cílem měl být jednoduchý program, který sleduje změny souborů v požadovaném adresáři.

Při prvním skenování požadovaného adresáře se do adresáře samotného vytvoří soubor `.pseudogit`, do kterého se zaznamenávají změny v adresáři.

**Zaznamenává se:**
1) přidání souboru
2) změna souboru a jeho verze (počínaje číslem 1)
3) odstranění souboru

_Poznámka k testování programu: Každé vytvoření `.pseudogit` souboru se zaznamenává do logu, který se ukládá na plochu uživatele. Tento log slouží pro
dva doplňující endpointy popsanými níže._

<br>

## Jak to vypadá

_Inicializace sledovaní změn:_

<img width="679" alt="image" src="https://user-images.githubusercontent.com/91826791/178426367-576f1c23-fef8-45df-9bed-264d7968770c.png">
<br>


_Zaznamenaní změny:_

<img width="643" alt="image" src="https://user-images.githubusercontent.com/91826791/178426687-d47bebf5-301b-45b0-8b26-c7e3a7808fc0.png">
<br>

_Beze změny:_

<img width="656" alt="image" src="https://user-images.githubusercontent.com/91826791/178427649-67956fea-76f9-42c2-8290-404b252d6e89.png">
<br>

## Stack
  - API: C#/.Net 6, ASP.Net Core
  - Klient: ASP.Net Core Blazor Server


## Doplňující funkce
Testování programu způsobí ukládání `.pseudogit` souborů. Těchto soborů může být více a je zbytečné si jimi zaneřádit systém.
V programu jsou naimplementováný dva doplňující endpointy:

- `/pseudogitfiles` - vrátí seznam všech vytvořených `.pseudogit` souborů
- `/clean` - všechny doposud vytvořené `.pseudogit`soubory se **odstraní**

K těmto endpointům nejsou v klientovi zatím naimplementovány žádné "čudlíky" a je nutné vyslat požadavek manuálně (browser, Postman, curl...)


## Použití
Doporučuji skenovat adresář dle **absolutní cesty** k souboru. Validace cesty pro Windows platformy není zatím naimplementována, pouze Unix-like systémy. 

## Podrobněji
Program je rozdělen na serverovou a klientskou část, kdy veškerá logika se zpracovává na serverové straně. Obě části mezi sebou komunikují prostřednictím
http.

Pro kontrolu změn v samotných souborech se používá porovnání aktuálních SHA1 hashů a těch zaznamenaných při minulém skenování. 
Pro tyto účely zcela dostačující. 


## Potřebné úpravy

#### Nezbytné
- Doplnit zobrazení kolikrát došlo ke změně souborů (tz. verze souborů)
  - Verze se **zaznamenávají** a **aktualizují**. Chybí "pouze" jejich zobrazení v klientovi.

#### Vhodné
- Dolnit validaci cesty k souboru na Windows platformách - validace na Unix-like systémech je naimplementována

#### Bylo by pěkné
- UX: Implementace "loading" komponenty při odeslání požadavku a čekání na odpověď
- UX & Performance: Implementace stránkování v klientovi v případě velkého množství změn v adresáři
- Možnost ignorovat sledování změn některých souborů
  
## Možná omezení a nedostatky
- testováno zatím pouze na MacOS
- logika validace cesty by měla být propracovanější, ale pro účely ukázky práce je tato, domnívám se, dostačující
