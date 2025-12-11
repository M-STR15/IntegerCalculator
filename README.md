# IntegratedCalCulator (ve vývoji)

💡 *Demonstrační aplikace pro výpočet jednoduchých integer hodnot.Projekt slouží jako ukázka architektury, práce s  WPF a architekturou MVVM.*

## Popis projektu



---

## Použité technologie

### Backend

- **.NET 8**
- **Ninject**
- **CommunityToolkit.Mvvm**

### Frontend
- **WPF** 
---
## Architektura

Aplikace využívá vícevrstvou architekturu podle principů **MVVM**.
Projekt je rozdělen na samostatné části pro lepší přehlednost a údržbu:

# Solution

- **BE (backendová část)**
  - **EventLog** – služba zajisťující logování událostí
  - **ExpressionEvaluator** – služba pro vyhodnocování výrazů případné generování vzorů
  - **Shared** – sdílené objekty

- **FE (frontendová část)**
  - **UIComponents** – WPF komponenty

---

## Nasazení


---    
## Poznámky

Projekt byl psán, aby při při generování výsledků vzorzů, aplikace zůstala funkční bez zamrznutí UI vlákna.

---

## Časová náročnost vývoje

| Fáze práce             | Čas |
|------------------------|------:|
| Přípravná práce        | 0,5 h |
| Vývoj (BE + FE)        | 10 h |
| Dokumentace            | 1 h |
| Testování              | 2 h |
| **Celkem:**            | **13,5 h** |

---


## Changes (verzování)

Legend: 

Hlavní kategorie 

- 🚀 - Nová funkce
- 🐞 - Oprava chyby
- 📝 - Dokumentace
- 🛠 - Úprava kódu
- 🚨 - Bezpečnostní aktualizace
- ❌ - Odstranění funkce
- 🛢 - Databázové úpravy

Dodatečné info:
- 🔒 - Nezveřejňovat informaci zákazníkovi
- 🔥 – Kritická
- ⚠ – Důležitá -> ovlivňující mnoho uživatelů
- 🛑 – Zásadní
- 🚨 – Bezpečnostní


***
### 0.0.2   (2025-12-11)
- 🛠 Kontrola zda existuje výstupní soubor
- 🛠 Ošetření dělení nulou
- 🛠 Úprava UI, aby při dlouhých výrazech nedocházelo, že nebude vidět celý výraz
- 🛠 🔒 Aplikace již při dlouhých výrazech nebude zasekávat, byla změněna funkce ze synchroní na asynchroní
### 0.0.1   (2025-12-04)
- 🚀 vytvořené základní funkcionality aplikace dle požadavku projektu
    - v dané verzi by bylo ještě dobré dodělat pár drobností:
        - optimalizovat UI, aby byli ošetřeny všechny vstupní podmíny uživatele a nedocházelo, tak k některému nechtěnému chování uživatele
        - případné optimalizování, aby šlo generovat vzorce


---

## Authors

[@M-STR](https://github.com/M-STR15)

## License

[MIT](https://choosealicense.com/licenses/mit/)