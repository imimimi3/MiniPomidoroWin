<div align="center">
  <img src="Accets/tomato.png" width="96" alt="MiniPomodoroWin" />

  # MiniPomodoroWin

  Минималистичный помодоро-таймер для Windows: круговой прогресс,
  компактное окно поверх всех окон и значок в трее.

  [![Latest release](https://img.shields.io/github/v/release/imimimi3/MiniPomidoroWin?style=flat-square&label=release)](https://github.com/imimimi3/MiniPomidoroWin/releases/latest)
  [![Downloads](https://img.shields.io/github/downloads/imimimi3/MiniPomidoroWin/total?style=flat-square)](https://github.com/imimimi3/MiniPomidoroWin/releases)
  ![Platform](https://img.shields.io/badge/platform-Windows%2010%2F11%20x64-0078D4?style=flat-square&logo=windows&logoColor=white)
  ![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)
  [![License: MIT](https://img.shields.io/badge/license-MIT-green?style=flat-square)](LICENSE)
</div>

---

## Возможности

- **Таймер помодоро** — минуты и секунды, до 100 минут.
- **Круговой прогресс** в мини-окне, которое всегда поверх остальных окон.
- **Пауза, продолжение и остановка** одним нажатием.
- **Сворачивание в трей и подсказка в трее**.
- **Напоминание раз в 10 минут**, если таймер не запущен.
- **Сообщение об окончании** «Пора переключиться!» поверх всех окон.
- **Своя мелодия окончания** (mp3, wav, aiff) с предпросмотром.
- **Русский и английский** с переключением без перезапуска.
- **Один `.exe` без установщика** — .NET устанавливать не нужно.

## Скриншоты

<p align="center">
  <img width="300" alt="Главное окно" src="https://github.com/user-attachments/assets/d85486f0-598a-4493-9f74-83b932cd3bc1" />
  <img width="300" alt="Настройки" src="https://github.com/user-attachments/assets/d377ec04-30f0-4d91-b152-e80e30526ca2" />
  <img width="300" alt="Информация" src="https://github.com/user-attachments/assets/2b1547ec-0d44-4670-89e7-f02e8ba40dea" />
  <img width="170" alt="Мини-окно" src="https://github.com/user-attachments/assets/20b88d57-ecab-42f6-b432-0598a5874c5e" />
</p>

## Установка

1. Откройте [последний релиз](https://github.com/imimimi3/MiniPomidoroWin/releases/latest).
2. Скачайте `MiniPomodoroWin.exe`.
3. Запустите — установка не требуется, права администратора не нужны.

> **Требования:** Windows 10 или 11, x64.
> Сборка самодостаточная (`self-contained`), .NET Runtime не нужен.
>
> **SmartScreen:** файл не подписан цифровой подписью, поэтому Windows может показать
> предупреждение. Нажмите «Подробнее» → «Выполнить в любом случае».

Настройки хранятся в профиле пользователя:

```text
%LOCALAPPDATA%\MiniPomidoroWin\
```

## Как пользоваться

1. Задайте минуты и секунды — стрелками, колесом мыши или клавишами `↑` `↓`, `Page Up` `Page Down`, `Home` `End`.
2. Нажмите круглую кнопку **Старт**.
3. В мини-окне: пауза/продолжение, стоп (`Esc`) и сворачивание в трей.
4. Двойной клик по значку в трее возвращает окно; в контекстном меню — «Развернуть» и «Выход».
5. Крестик главного окна не закрывает приложение, а прячет его в трей.
6. По окончании таймера появляется сообщение и звучит мелодия.

## Настройки

| Раздел | Что можно сделать |
|---|---|
| **Звук окончания** | Выбрать свой файл с предпросмотром, вернуть стандартный звук |
| **Язык интерфейса** | Переключить русский/английский на лету |

## Сборка из исходников

Понадобится [.NET 9 SDK](https://dotnet.microsoft.com/download) и Windows.

```powershell
git clone https://github.com/imimimi3/MiniPomidoroWin.git
cd MiniPomidoroWin

# отладка
dotnet build
dotnet run

# релизная сборка одним файлом
dotnet publish MiniPomidoroWin.csproj -c Release -r win-x64 --self-contained true -o publish
```

Результат — `publish\MiniPomidoroWin.exe`, единственный файл.

## Структура проекта

```text
Forms/                  окна и локализация интерфейса
  Localization/         строки RU/EN
Controls/               кастомные контролы (кнопки, круговой таймер, поле времени)
Service/                звук, настройки, локализация, трей, конфигурация
Themes/                 палитра приложения
Constants/              константы
Properties/             ресурсы и настройки
```

## Технологии

- C# 12, .NET 9, Windows Forms
- [NAudio](https://github.com/naudio/NAudio) — воспроизведение мелодии
- Собственная отрисовка контролов на GDI+
- Single-file self-contained publish (`win-x64`)

## Планы

- [ ] Глобальные горячие клавиши
- [ ] Защита от запуска второй копии
- [ ] Автозапуск с Windows
- [ ] Настройка интервала напоминаний
- [ ] Регулировка громкости
- [ ] Автотесты и CI для сборки
- [ ] Подпись исполняемого файла

## Лицензия

Проект распространяется по лицензии [MIT](LICENSE).

## English

**MiniPomodoroWin** is a minimalist Pomodoro timer for Windows: a circular
progress mini-window that stays on top, tray integration with a remaining-time
tooltip and 10-minute reminders, a "time to switch" message shown before the
end sound, custom end melody, and live RU/EN switching.

Download the single `MiniPomodoroWin.exe` from
[Releases](https://github.com/imimimi3/MiniPomidoroWin/releases/latest) —
no installer, no .NET runtime required (Windows 10/11 x64).

Build:

```powershell
dotnet publish MiniPomidoroWin.csproj -c Release -r win-x64 --self-contained true -o publish
```
