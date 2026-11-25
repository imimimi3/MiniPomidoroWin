<div align="center">
  <img src="Accets/tomato.png" width="96" alt="MiniPomidoroWin" />

  # MiniPomidoroWin

  Минималистичный помодоро-таймер для Windows: круговой прогресс,
  компактное окно поверх всех окон, значок в трее и автозапуск.

  [![Latest release](https://img.shields.io/github/v/release/imimimi3/MiniPomidoroWin?style=flat-square&label=release)](https://github.com/imimimi3/MiniPomidoroWin/releases/latest)
  [![Downloads](https://img.shields.io/github/downloads/imimimi3/MiniPomidoroWin/total?style=flat-square)](https://github.com/imimimi3/MiniPomidoroWin/releases)
  ![Platform](https://img.shields.io/badge/platform-Windows%2010%2F11%20x64-0078D4?style=flat-square&logo=windows&logoColor=white)
  ![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)
  [![License: MIT](https://img.shields.io/badge/license-MIT-green?style=flat-square)](LICENSE)
</div>

---

## Возможности

- **Таймер помодоро** — до 100 минут; время вводится цифрами с клавиатуры, стрелками или колесом мыши.
- **Круговой прогресс** в мини-окне, которое всегда поверх остальных окон.
- **Пауза, продолжение и остановка** одним нажатием; `Esc` останавливает и закрывает мини-окно.
- **Сворачивание в трей**: подсказка с оставшимся временем и меню «Развернуть» / «Выход».
- **Напоминание после 10 минут простоя** — отсчёт начинается заново после завершения или остановки таймера, а не от запуска приложения.
- **Сообщение об окончании** «Пора переключиться!» поверх всех окон.
- **Своя мелодия окончания** (mp3, wav, aiff) с предпросмотром.
- **Русский и английский** с переключением без перезапуска.
- **Автозапуск вместе с Windows** — при входе приложение стартует сразу в трее; права администратора не нужны.
- **Одна копия приложения** — повторный запуск показывает уже открытое окно, второй процесс не создаётся.
- **Подстройка под экран** — окна не выходят за рабочую область, слишком длинный текст уменьшается автоматически.
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
2. Скачайте `MiniPomidoroWin.exe`.
3. Запустите — установка не требуется, права администратора не нужны.

> **Требования:** Windows 10 или 11, x64.
> Сборка самодостаточная (`self-contained`), .NET Runtime не нужен.
>
> **SmartScreen:** файл не подписан цифровой подписью, поэтому Windows может показать
> предупреждение. Нажмите «Подробнее» → «Выполнить в любом случае».
> Если не доверяете готовому бинарнику — соберите его из исходников (см. ниже).

Настройки хранятся в профиле пользователя:

```text
%LOCALAPPDATA%\MiniPomidoroWin\
```

Параметр автозапуска — в реестре текущего пользователя (см. раздел
[«Автозапуск и единственный экземпляр»](#автозапуск-и-единственный-экземпляр)).
Отдельного установщика и деинсталлятора нет: достаточно выключить автозапуск и удалить exe.

## Как пользоваться

1. Задайте время: наберите число цифрами — первая цифра заменяет текущее значение, следующие добавляются. `Backspace` удаляет последнюю цифру, `Enter`, `Esc` или `Tab` фиксируют ввод. Также работают `↑` `↓` (±1), `Page Up` `Page Down` (±10), `Home` `End` (минимум/максимум) и колесо мыши.
2. Нажмите круглую кнопку **Старт**.
3. В мини-окне: пауза/продолжение, стоп (`Esc`) и сворачивание в трей.
4. Двойной клик по значку в трее возвращает окно; в контекстном меню — «Развернуть» и «Выход».
5. Крестик главного окна не закрывает приложение, а прячет его в трей.
6. По окончании таймера появляется сообщение, затем звучит мелодия.
7. Если таймер не запущен, раз в 10 минут приходит напоминание. Отсчёт начинается заново после завершения или остановки таймера.
8. Повторный запуск exe при работающем приложении просто показывает уже открытое окно: второй процесс не создаётся, отсчёт таймера не сбрасывается.

## Настройки

| Раздел | Что можно сделать |
|---|---|
| **Звук окончания** | Выбрать свой файл с предпросмотром, вернуть стандартный звук |
| **Язык интерфейса** | Переключить русский/английский на лету |
| **Автозапуск** | Включить или выключить запуск вместе с Windows |

## Автозапуск и единственный экземпляр

Автозапуск настраивается в окне настроек и не требует прав администратора: приложение добавляет значение `MiniPomidoroWin` в раздел реестра текущего пользователя

```text
HKCU\Software\Microsoft\Windows\CurrentVersion\Run
```

Команда запуска содержит путь к exe в кавычках и аргумент `--tray`:

```text
"C:\путь\к\MiniPomidoroWin.exe" --tray
```

- `--tray` — старт сразу в трее, без показа главного окна. Этот же аргумент можно использовать вручную, например в ярлыке.
- Если exe перенести в другую папку, путь в реестре обновится при следующем обычном запуске.
- Отключить автозапуск можно как в настройках приложения, так и в
  «Диспетчер задач → Автозагрузка» или «Параметры → Приложения → Автозапуск».
  Приложение учитывает состояние, выставленное в Windows, и показывает актуальное значение.
- Если удаляете приложение вручную, сначала выключите автозапуск в настройках либо
  удалите значение `MiniPomidoroWin` из указанного раздела реестра.

Единственный экземпляр обеспечивается именованным мьютексом. Второй запуск передаёт первому сигнал «покажи окно» и сразу завершается, поэтому в Диспетчере задач всегда ровно один процесс `MiniPomidoroWin.exe`.

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

Результат — `publish\MiniPomidoroWin.exe`, единственный файл. При необходимости номер версии задаётся параметром `-p:Version=1.2.3`.

## Структура проекта

```text
Forms/                  окна и локализация интерфейса
  Localization/         строки RU/EN
Controls/               кастомные контролы (кнопки, круговой таймер, поле времени, автоподбор размера текста)
Service/                звук, настройки, локализация, автозапуск, единственный экземпляр, извлечение конфигурации
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
- [ ] Настройка интервала напоминаний
- [ ] Регулировка громкости
- [ ] Цифровая подпись релизов
- [ ] Автотесты и CI для сборки

## Лицензия

Проект распространяется по лицензии [MIT](LICENSE).

## English

**MiniPomidoroWin** is a minimalist Pomodoro timer for Windows: a circular
progress mini-window that stays on top, tray integration with a remaining-time
tooltip and idle reminders, a "time to switch" message shown before the end sound, custom end melody, live RU/EN switching, Windows autostart and single-instance behavior.
- **Timer** — up to 100 minutes; type the time with the keyboard or use the arrows and mouse wheel.
- **Reminders** — 10 minutes of idle time counted from the moment the timer finished or was stopped, not from app start.
- **Tray** — collapsing, remaining-time tooltip, 10-minute reminder balloon.
- **Autostart** — optional, starts hidden in the tray at sign-in (HKCU, no admin rights required).
- **Single instance** — launching the exe again brings the existing window back instead of starting a second process.
- **Screen fit** — windows stay inside the working area and oversized text is shrunk automatically.

Download the single `MiniPomidoroWin.exe` from
[Releases](https://github.com/imimimi3/MiniPomidoroWin/releases/latest) —
no installer, no .NET runtime required (Windows 10/11 x64).

Build:

```powershell
dotnet publish MiniPomidoroWin.csproj -c Release -r win-x64 --self-contained true -o publish
```
