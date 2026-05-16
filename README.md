# Dice Game

Implementacja gry w kości (Yahtzee) w stylu retro, zbudowana przy użyciu **C# 12**, **SadConsole 10** oraz **MonoGame**.

## Główne cechy techniczne

- **Architektura warstwowa**: Wyraźne oddzielenie logiki gry (`Logic/`) od warstwy prezentacji (`Components/`).
- **Zarządzanie pamięcią**: Implementacja interfejsu `IDisposable` w komponentach UI w celu eliminacji wycieków pamięci przy subskrypcji zdarzeń.
- **Logika punktacji**: Pełne pokrycie testami jednostkowymi klasy `ScoreCalculator` (obsługa wszystkich układów i progów premiowych).
- **Deduplikacja UI**: Wydzielenie wspólnej logiki animacji (miganie) oraz systemu motywów (`Theme.cs`).
- **Orkiestracja**: Stabilne zarządzanie ekranami oparte na zdarzeniach i zapewnienie poprawnego zamykania aplikacji.

## Struktura projektu

- **DiceGame.sln** – Główne rozwiązanie łączące projekt gry i testy.
- **DiceGame/** – Folder główny aplikacji:
  - **Logic/** – Czysta logika biznesowa: zasady gry, obliczenia punktowe i zarządzanie sesją (brak zależności od UI).
  - **Components/** – Komponenty interfejsu użytkownika, system motywów oraz pomocnicze klasy renderujące.
  - **Scenes/** – Główne ekrany aplikacji (Menu Główne, Ekran Gry).
  - **Assets/** – Zasoby zewnętrzne: czcionki, efekty dźwiękowe oraz muzyka.
- **DiceGame.Tests/** – Testy jednostkowe dla kluczowych mechanik punktowania.

## Decyzje projektowe

### Architektura warstwowa

Zasady gry i obliczenia punktowe zostały umieszczone w przestrzeni `Logic/`, która nie posiada żadnych zależności od silnika graficznego. Warstwa prezentacji (`Components/`) odpowiada wyłącznie za renderowanie i obsługę wejścia. Dzięki temu separacja umożliwiła stworzenie testów jednostkowych bez potrzeby uruchamiania silnika gry.

### Wspólna klasa bazowa UI

Wszystkie panele interfejsu dziedziczą po abstrakcyjnej klasie `BasePanel`, która dostarcza współdzielone metody renderowania (ramki, centrowanie tekstu) oraz jednolity system motywów. Eliminuje to duplikację kodu wizualnego i zapewnia spójny wygląd interfejsu.

### Cykl życia UI oparty na zdarzeniach

Komunikacja między stanem gry a interfejsem odbywa się poprzez zdarzenia (events). Aby zapobiec wyciekom pamięci, wprowadzono mechanizm wyrejestrowywania zdarzeń. Komponenty UI implementują `IDisposable`, co zapewnia odpięcie handlerów przy przejściu między ekranami (np. podczas powrotu z gry do menu głównego).

### Centralizacja stałych i konfiguracji

Wszystkie kluczowe parametry projektu, takie jak wymiary interfejsu, progi punktowe czy wartości premii, zostały zgrupowane w dedykowanych klasach statycznych (`GameSettings.cs`, `ScoreCategory.cs`). Zapewnia to jedno źródło prawdy i pozwala na zmianę parametrów gry bez modyfikowania logiki w wielu miejscach kodu.

## Music Attribution

"Bit Shift" Kevin MacLeod ([incompetech.com](https://incompetech.com/music/royalty-free/index.html?isrc=USUAN1600045&Search=Search))  
Licensed under Creative Commons: By Attribution 4.0 License  
http://creativecommons.org/licenses/by/4.0/  
Format converted from MP3 to WAV for engine compatibility and shortened to around 1 minute.

## Dodatkowe zasoby

### Efekty dźwiękowe

- Dźwięki retro (rzut, blokada, wybór) wygenerowano przy użyciu narzędzia **[Bfxr](https://www.bfxr.net/)**.

### Czcionka

- **Cheepicus 12x12**: Standardowa czcionka SadConsole.

## Kompilacja i uruchomienie

1. Upewnij się, że masz zainstalowane **.NET 8 SDK**.
2. Otwórz terminal w **głównym folderze repozytorium**.
3. Uruchom aplikację:
   ```bash
   dotnet run --project DiceGame
   ```
4. (Opcjonalnie) Uruchom testy:
   ```bash
   dotnet test
   ```

## Instrukcja gry (Zasady)

1. **Początek gry**: W menu głównym należy wybrać liczbę graczy (**od 2 do 4**). Gra automatycznie dostosuje szerokość interfejsu do liczby uczestników.
2. **Cel gry**: Zdobądź jak najwięcej punktów, wypełniając 13 kategorii na tablicy wyników.
3. **Tura**: W każdej turze gracz ma do dyspozycji maksymalnie **3 rzuty**.
4. **Zatrzymywanie kości**: Po pierwszym i drugim rzucie można kliknąć na kości, które mają zostać zachowane (zostaną one "zamrożone" na następny rzut).
5. **Zapisywanie punktów**: Po zakończeniu rzutów (lub wcześniej, jeśli wynik jest satysfakcjonujący) należy kliknąć na wybraną kategorię w tabeli wyników, aby przypisać do niej punkty.
6. **Kategorie**: Każda kategoria może być użyta tylko raz. Jeśli układ nie pasuje do żadnej kategorii, konieczne jest wpisanie "0" do jednej z nich.
7. **Premia**: Jeśli suma punktów w górnej sekcji (1-6) wyniesie co najmniej 63, gracz otrzymuje dodatkowe 35 punktów premii.
