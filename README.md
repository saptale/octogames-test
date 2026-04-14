# Unity Technical Test - Middle Unity Developer
---

`Код заданий находится в папке Assets/Tasks и имеет название соответствующее заданию`

`The code for the tasks is located in the Assets/Tasks folder and is named according to each task`

# Rus

## 1. Принципы кодирования

При реализации я руководствовался двумя основными принципами:

Разделение ответственности: Геймплейная логика отделена от UI. Компоненты интерфейса выступают только в роли View, что предотвращает появление запутанного кода где UI напрямую меняет состояние игровых объектов.

Data-Driven Design: Поведение систем (кнопки попапов, интервалы обновлений) настраивается через структуры данных и поля инспектора. Это позволяет дизайнерам менять контент и баланс без участия программиста.

## 2. Утилита сохранения и загрузки

Реализована универсальная система `SaveSystem` на базе JSON.

Надежность: Обработка исключений при доступе к файлам, использование временных файлов (`.tmp`) и резервных копий (`.bak`) для защиты от повреждения данных.

Универсальность: Поддержка любого сериализуемого класса через реализацию интерфейса `ISaveable`.

API: Централизованная регистрация объектов и асинхронные (`awaitable`) методы `Save()` / `Load()` для всей системы без зависаний основного потока.

## 3. Система PopUp / UI

Архитектура для модальных окон, диалогов и туториалов.

Логика: Использование очереди для последовательного показа окон и пула объектов для оптимизации памяти.

Загрузка: Асинхронная загрузка через Unity Addressables.

Компоненты префаба:
TextMeshProUGUI для текста.
Layout Groups и Content Size Fitter для динамического изменения размера окна под количество текста и кнопок (от 1 до 5).

## 4. Производительность UI и рефакторинг

Проведен рефакторинг скрипта `CharactersView`.

Оптимизация: Перенос логики из `FixedUpdate` в кастомный Update Manager с настраиваемой частотой обновления.

Паттерн Реестр: Замена дорогого поиска компонентов в каждом кадре на систему саморегистрации объектов, к примеру в (`OnEnable`/`OnDisable`).

Логика: Исправлены математические ошибки (расчет среднего значения) и добавлена защита от деления на ноль.

## 5. Геймплейная логика и состояния

Система трекинга активных сущностей на сцене.

Архитектура: Использование `HashSet<T>` обеспечивает мгновенный поиск и удаление объектов.

Производительность: Система работает в рантайме, предоставляя доступ к коллекции через `IReadOnlyCollection` без лишних аллокаций памяти.

Жизненный цикл: Автоматическая очистка статических данных при перезапуске сцены и корректная обработка отключенных объектов.

### Ответы на дополнительные вопросы (Optional Bonus)

How would you scale these systems for larger projects?

Я бы внедрил Dependency Injection (VContainer/Zenject) для управления зависимостями вместо синглтонов. Также разделил бы SaveSystem на поддержку нескольких слотов сохранений и раздельных файлов (например, профиль игрока и данные мира). Для UI использовал бы State Machine, чтобы управлять стеком окон.

How would designers interact with this code?

Дизайнеры взаимодействовали бы с системой преимущественно через Инспектор. Я бы вынес настройки в `ScriptableObjects` (пресеты попапов, параметры баланса) и использовал кастомные Editor скрипты (например написанная мною таблица для балансировки: https://github.com/saptale/ScriptableObject-Spreadsheet-Unity-Editor.git), чтобы они могли визуально отслеживать состояние систем.

How would you profile or debug performance issues?

Я бы использовал Unity Profiler, уделяя особое внимание вкладкам CPU Usage (поиск тяжелых скриптов) и GC Alloc (поиск создания мусора в циклах). Для оптимизации UI и графического пайплайна я бы применил Frame Debugger, чтобы минимизировать количество Draw Calls и количество проходов. Для поиска утечек памяти отлично подходит пакет Memory Profiler, с помощью которого можно находить зависшие ссылки.

---

# Eng

## 1. Coding Principles

In this project, I followed two key principles to ensure scalability and designer-friendly iterations:

Separation of Concerns: Gameplay logic is decoupled from the UI. UI components act solely as "Views," preventing the creation of "spaghetti code" where the UI directly modifies the state of game objects.

Data-Driven Design: System behaviors (popup buttons, update intervals) are configured via data structures and Inspector fields. This allows designers to iterate on content and balance without a programmer's involvement.

## 2. Save / Load Utility

Implemented a generic JSON-based `SaveSystem`.

Robustness: Exception handling during file access, use of temporary (`.tmp`) and backup (`.bak`) files to protect against data corruption.

Universality: Supports any serializable class through the implementation of the `ISaveable` interface.

API: Centralized object registration and asynchronous (awaitable) `Save()` / `Load()` methods for the entire system, preventing main thread freezes.

## 3. Popup / UI System

Architecture for modal windows, dialogs, and tutorials.

Logic: Uses a queue for sequential window display and an object pool to optimize memory usage.

Loading: Asynchronous loading via Unity Addressables.

Prefab Components:
TextMeshProUGUI for text rendering.
Layout Groups and Content Size Fitter for dynamic window resizing based on text volume and button count (from 1 to 5).

## 4. UI Performance & Refactoring

Refactored the `CharactersView` script.

Optimization: Moved logic from `FixedUpdate` to a custom Update Manager with adjustable update frequencies.

Registry Pattern: Replaced expensive component searches every frame with an object self-registration system (e.g., in `OnEnable`/`OnDisable`).

Logic: Fixed mathematical errors (average value calculation) and added divide-by-zero protection.

## 5. Gameplay Logic and States

A tracking system for active entities in the scene.

Architecture: Using `HashSet<T>` provides instant object lookup and removal.

Performance: The system runs at runtime, providing access to the collection via `IReadOnlyCollection` without unnecessary memory allocations.

Lifecycle: Automatic clearing of static data on scene restart and proper handling of disabled objects.

### Optional Bonus Answers

How would you scale these systems for larger projects?

I would implement Dependency Injection (e.g., VContainer or Zenject) to manage dependencies instead of using Singletons. I would also expand the `SaveSystem` to support multiple save slots and separate files (e.g., separating player profiles from world data). For the UI, I would implement a State Machine to manage the window stack more effectively.

How would designers interact with this code?

Designers would interact with the system primarily through the Inspector. I would move settings into `ScriptableObjects` (for popup presets and balance parameters) and use custom Editor scripts (such as the spreadsheet-based balancing table I developed: https://github.com/saptale/ScriptableObject-Spreadsheet-Unity-Editor.git) to allow them to visually track the state of systems.

How would you profile or debug performance issues?

I would use the Unity Profiler, paying close attention to the CPU Usage tab (to find heavy scripts) and GC Alloc (to identify garbage creation in loops). For UI and graphics pipeline optimization, I would use the Frame Debugger to minimize Draw Calls and the number of passes. To find memory leaks, the Memory Profiler package is ideal for locating "dangling" references.