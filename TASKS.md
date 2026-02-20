# Development Tasks - Desktop Check Print Master

**Project:** Desktop Check Print & Calibration Master  
**Version:** 1.0.0  
**Timeline:** 4 Weeks (Sprint-based)  
**Last Updated:** 19 Şubat 2026

---

## Task Legend

- 🔴 **Critical (P0)** - Blocker, must be completed
- 🟡 **High (P1)** - Important, should be completed
- 🟢 **Medium (P2)** - Nice to have
- ⚪ **Low (P3)** - Future enhancement

**Status:**
- ⬜ Not Started
- 🟦 In Progress
- ✅ Completed
- 🚫 Blocked
- ⏸️ On Hold

---

## SPRINT 1 - Foundation & Core Logic (Week 1)

### Phase 1.1: Project Setup & Infrastructure

| ID | Task | Priority | Status | Assignee | Est. |
|--- |------|----------|--------|----------|------|
| T001 | Create solution structure (CheckPrintApp.sln) | 🔴 | ⬜ | Dev | 0.5h |
| T002 | Create Core project (CheckPrintApp.Core) | 🔴 | ⬜ | Dev | 0.5h |
| T003 | Create UI project (CheckPrintApp.UI - WPF) | 🔴 | ⬜ | Dev | 0.5h |
| T004 | Create Test project (CheckPrintApp.Tests - xUnit) | 🔴 | ⬜ | Dev | 0.5h |
| T005 | Setup NuGet packages (System.Text.Json, Serilog, etc.) | 🔴 | ⬜ | Dev | 1h |
| T006 | Configure Dependency Injection (Microsoft.Extensions.DI) | 🔴 | ⬜ | Dev | 1h |
| T007 | Setup logging infrastructure (Serilog) | 🟡 | ⬜ | Dev | 1h |
| T008 | Create .gitignore and initialize Git repository | 🟡 | ⬜ | Dev | 0.5h |
| T009 | Setup project folder structure (Models, Services, etc.) | 🔴 | ⬜ | Dev | 0.5h |

**Sprint 1.1 Total:** ~6 hours

---

### Phase 1.2: Core Models Implementation

| ID | Task | Priority | Status | Assignee | Est. |
|--- |------|----------|--------|----------|------|
| T010 | Create `CheckModel.cs` | 🔴 | ⬜ | Dev | 1h |
| T011 | - Add properties: Date, PayeeName, Amount, Location | 🔴 | ⬜ | Dev | - |
| T012 | - Add computed property: AmountInWords | 🔴 | ⬜ | Dev | - |
| T013 | - Add data validation attributes | 🟡 | ⬜ | Dev | - |
| T014 | Create `CalibrationConfig.cs` | 🔴 | ⬜ | Dev | 1h |
| T015 | - Add properties: OffsetX, OffsetY | 🔴 | ⬜ | Dev | - |
| T016 | - Add properties: DefaultLocation, FontFamily, FontSize | 🔴 | ⬜ | Dev | - |
| T017 | - Add properties: AutoUpperCase | 🔴 | ⬜ | Dev | - |
| T018 | Create `AppSettings.cs` | 🔴 | ⬜ | Dev | 1h |
| T019 | - Add properties: Version, Calibration, General, LastCheck | 🔴 | ⬜ | Dev | - |

**Sprint 1.2 Total:** ~3 hours

---

### Phase 1.3: Number to Text Converter (Critical)

| ID | Task | Priority | Status | Assignee | Est. |
|--- |------|----------|--------|----------|------|
| T020 | Create `INumberToTextConverter.cs` interface | 🔴 | ⬜ | Dev | 0.5h |
| T021 | Create `NumberToTextConverter.cs` class | 🔴 | ⬜ | Dev | 4h |
| T022 | - Implement integer part conversion (0-999) | 🔴 | ⬜ | Dev | - |
| T023 | - Implement thousands conversion (1.000-999.999) | 🔴 | ⬜ | Dev | - |
| T024 | - Implement millions conversion (1.000.000+) | 🔴 | ⬜ | Dev | - |
| T025 | - Implement decimal part (kuruş) conversion | 🔴 | ⬜ | Dev | - |
| T026 | - Add "Yalnız" prefix | 🔴 | ⬜ | Dev | - |
| T027 | - Add "Türk Lirası" and "Kuruş" suffixes | 🔴 | ⬜ | Dev | - |
| T028 | - Convert to uppercase | 🔴 | ⬜ | Dev | - |
| T029 | Create unit tests for NumberToTextConverter | 🔴 | ⬜ | Dev | 3h |
| T030 | - Test case: 0,00 TL | 🔴 | ⬜ | Dev | - |
| T031 | - Test case: 0,50 TL | 🔴 | ⬜ | Dev | - |
| T032 | - Test case: 1,00 TL | 🔴 | ⬜ | Dev | - |
| T033 | - Test case: 1.250,50 TL | 🔴 | ⬜ | Dev | - |
| T034 | - Test case: 1.000.000,00 TL | 🔴 | ⬜ | Dev | - |
| T035 | - Test case: 999.999.999,99 TL (max) | 🔴 | ⬜ | Dev | - |
| T036 | - Test edge cases (9, 10, 100, 1000) | 🔴 | ⬜ | Dev | - |

**Sprint 1.3 Total:** ~7.5 hours

---

### Phase 1.4: Helper Classes

| ID | Task | Priority | Status | Assignee | Est. |
|--- |------|----------|--------|----------|------|
| T037 | Create `UnitConverter.cs` helper class | 🔴 | ⬜ | Dev | 1h |
| T038 | - Implement MmToPixel() method | 🔴 | ⬜ | Dev | - |
| T039 | - Implement PixelToMm() method | 🔴 | ⬜ | Dev | - |
| T040 | - Add DPI constant (96 for WPF) | 🔴 | ⬜ | Dev | - |
| T041 | Create unit tests for UnitConverter | 🔴 | ⬜ | Dev | 1h |
| T042 | Create `ValidationHelper.cs` | 🟡 | ⬜ | Dev | 1h |
| T043 | - Add ValidateAmount() method | 🟡 | ⬜ | Dev | - |
| T044 | - Add ValidateDate() method | 🟡 | ⬜ | Dev | - |
| T045 | - Add ValidatePayeeName() method | 🟡 | ⬜ | Dev | - |

**Sprint 1.4 Total:** ~3 hours

---

**SPRINT 1 TOTAL: ~19.5 hours**

---

## SPRINT 2 - UI Implementation & Data Binding (Week 2)

### Phase 2.1: MVVM Infrastructure

| ID | Task | Priority | Status | Assignee | Est. |
|--- |------|----------|--------|----------|------|
| T046 | Create `ViewModelBase.cs` | 🔴 | ⬜ | Dev | 1h |
| T047 | - Implement INotifyPropertyChanged | 🔴 | ⬜ | Dev | - |
| T048 | - Add OnPropertyChanged() helper | 🔴 | ⬜ | Dev | - |
| T049 | Create `RelayCommand.cs` (ICommand implementation) | 🔴 | ⬜ | Dev | 1h |

**Sprint 2.1 Total:** ~2 hours

---

### Phase 2.2: Main ViewModel

| ID | Task | Priority | Status | Assignee | Est. |
|--- |------|----------|--------|----------|------|
| T050 | Create `MainViewModel.cs` | 🔴 | ⬜ | Dev | 4h |
| T051 | - Add CheckModel property with INotifyPropertyChanged | 🔴 | ⬜ | Dev | - |
| T052 | - Add CalibrationConfig property | 🔴 | ⬜ | Dev | - |
| T053 | - Implement Date property with DatePicker binding | 🔴 | ⬜ | Dev | - |
| T054 | - Implement PayeeName property with auto-uppercase | 🔴 | ⬜ | Dev | - |
| T055 | - Implement Amount property with validation | 🔴 | ⬜ | Dev | - |
| T056 | - Implement Location property | 🔴 | ⬜ | Dev | - |
| T057 | - Implement AmountInWords (computed from Amount) | 🔴 | ⬜ | Dev | - |
| T058 | - Implement OffsetX property with slider binding | 🔴 | ⬜ | Dev | - |
| T059 | - Implement OffsetY property with slider binding | 🔴 | ⬜ | Dev | - |
| T060 | - Implement IsTestPrint checkbox binding | 🔴 | ⬜ | Dev | - |
| T061 | - Add PrintCommand | 🔴 | ⬜ | Dev | - |
| T062 | - Add SaveSettingsCommand | 🔴 | ⬜ | Dev | - |
| T063 | - Add ResetCalibrationCommand | 🔴 | ⬜ | Dev | - |
| T064 | - Add OpenSettingsCommand | 🟡 | ⬜ | Dev | - |

**Sprint 2.2 Total:** ~4 hours

---

### Phase 2.3: Main Window XAML

| ID | Task | Priority | Status | Assignee | Est. |
|--- |------|----------|--------|----------|------|
| T065 | Create MainWindow.xaml layout | 🔴 | ⬜ | Dev | 3h |
| T066 | - Create Grid with 2 columns (Input | Preview) | 🔴 | ⬜ | Dev | - |
| T067 | - Add "Çek Bilgileri" GroupBox | 🔴 | ⬜ | Dev | - |
| T068 | - Add DatePicker for Date | 🔴 | ⬜ | Dev | - |
| T069 | - Add TextBox for PayeeName | 🔴 | ⬜ | Dev | - |
| T070 | - Add TextBox for Amount (with currency formatting) | 🔴 | ⬜ | Dev | - |
| T071 | - Add TextBox for AmountInWords (read-only) | 🔴 | ⬜ | Dev | - |
| T072 | - Add TextBox for Location | 🔴 | ⬜ | Dev | - |
| T073 | - Add "Kalibrasyon Ayarı" GroupBox | 🔴 | ⬜ | Dev | - |
| T074 | - Add Slider for OffsetX (-50 to +50, step 0.5) | 🔴 | ⬜ | Dev | - |
| T075 | - Add Label for OffsetX value display | 🔴 | ⬜ | Dev | - |
| T076 | - Add Slider for OffsetY (-50 to +50, step 0.5) | 🔴 | ⬜ | Dev | - |
| T077 | - Add Label for OffsetY value display | 🔴 | ⬜ | Dev | - |
| T078 | - Add CheckBox for IsTestPrint | 🔴 | ⬜ | Dev | - |
| T079 | - Add "Varsayılana Dön" Button | 🟡 | ⬜ | Dev | - |
| T080 | - Add "Ayarları Kaydet" Button | 🟡 | ⬜ | Dev | - |
| T081 | - Add "YAZDIR" Button (prominent, primary action) | 🔴 | ⬜ | Dev | - |
| T082 | - Add "Ayarlar" Button | 🟡 | ⬜ | Dev | - |
| T083 | Setup data binding for all controls | 🔴 | ⬜ | Dev | 2h |

**Sprint 2.3 Total:** ~5 hours

---

### Phase 2.4: Preview Control

| ID | Task | Priority | Status | Assignee | Est. |
|--- |------|----------|--------|----------|------|
| T084 | Create `PreviewControl.xaml` UserControl | 🔴 | ⬜ | Dev | 4h |
| T085 | - Design check template layout | 🔴 | ⬜ | Dev | - |
| T086 | - Add Canvas for absolute positioning | 🔴 | ⬜ | Dev | - |
| T087 | - Add TextBlock for Date (with offset binding) | 🔴 | ⬜ | Dev | - |
| T088 | - Add TextBlock for PayeeName (with offset binding) | 🔴 | ⬜ | Dev | - |
| T089 | - Add TextBlock for Amount (with # guards and offset) | 🔴 | ⬜ | Dev | - |
| T090 | - Add TextBlock for AmountInWords (with offset) | 🔴 | ⬜ | Dev | - |
| T091 | - Add TextBlock for Location (with offset) | 🔴 | ⬜ | Dev | - |
| T092 | - Add check border/background image | 🟡 | ⬜ | Dev | - |
| T093 | Implement live preview update logic | 🔴 | ⬜ | Dev | 2h |
| T094 | - Bind to MainViewModel properties | 🔴 | ⬜ | Dev | - |
| T095 | - Apply calibration offsets in real-time | 🔴 | ⬜ | Dev | - |
| T096 | - Toggle between normal/test print view | 🔴 | ⬜ | Dev | - |

**Sprint 2.4 Total:** ~6 hours

---

### Phase 2.5: Value Converters

| ID | Task | Priority | Status | Assignee | Est. |
|--- |------|----------|--------|----------|------|
| T097 | Create `CurrencyConverter.cs` (IValueConverter) | 🔴 | ⬜ | Dev | 1h |
| T098 | - Convert decimal to formatted string (1250.50 -> "1.250,50") | 🔴 | ⬜ | Dev | - |
| T099 | - Add # guards in display | 🔴 | ⬜ | Dev | - |
| T100 | Create `MillimeterConverter.cs` (for offset display) | 🟡 | ⬜ | Dev | 1h |
| T101 | - Convert double to "±X.X mm" format | 🟡 | ⬜ | Dev | - |

**Sprint 2.5 Total:** ~2 hours

---

**SPRINT 2 TOTAL: ~19 hours**

---

## SPRINT 3 - Services & Core Functionality (Week 3)

### Phase 3.1: Settings Service

| ID | Task | Priority | Status | Assignee | Est. |
|--- |------|----------|--------|----------|------|
| T102 | Create `ISettingsService.cs` interface | 🔴 | ⬜ | Dev | 0.5h |
| T103 | Create `SettingsService.cs` | 🔴 | ⬜ | Dev | 3h |
| T104 | - Implement LoadSettings() - read from JSON | 🔴 | ⬜ | Dev | - |
| T105 | - Implement SaveSettings() - write to JSON | 🔴 | ⬜ | Dev | - |
| T106 | - Implement GetDefaultSettings() | 🔴 | ⬜ | Dev | - |
| T107 | - Handle file not found (first run) | 🔴 | ⬜ | Dev | - |
| T108 | - Handle JSON parsing errors | 🔴 | ⬜ | Dev | - |
| T109 | - Determine settings file path (AppData/roaming) | 🔴 | ⬜ | Dev | - |
| T110 | - Support portable mode (check exe directory) | 🟡 | ⬜ | Dev | - |
| T111 | Create unit tests for SettingsService | 🟡 | ⬜ | Dev | 2h |
| T112 | - Test save/load cycle | 🟡 | ⬜ | Dev | - |
| T113 | - Test missing file scenario | 🟡 | ⬜ | Dev | - |

**Sprint 3.1 Total:** ~5.5 hours

---

### Phase 3.2: Printer Service (Critical Component)

| ID | Task | Priority | Status | Assignee | Est. |
|--- |------|----------|--------|----------|------|
| T114 | Create `IPrinterService.cs` interface | 🔴 | ⬜ | Dev | 0.5h |
| T115 | Create `PrinterService.cs` | 🔴 | ⬜ | Dev | 8h |
| T116 | - Implement PrintCheck() method | 🔴 | ⬜ | Dev | - |
| T117 | - Create PrintDialog instance | 🔴 | ⬜ | Dev | - |
| T118 | - Configure page size (check dimensions) | 🔴 | ⬜ | Dev | - |
| T119 | - Define base coordinates for check fields | 🔴 | ⬜ | Dev | - |
| T120 | - Apply calibration offsets to coordinates | 🔴 | ⬜ | Dev | - |
| T121 | - Create DrawingVisual for rendering | 🔴 | ⬜ | Dev | - |
| T122 | - Implement DrawString for Date field | 🔴 | ⬜ | Dev | - |
| T123 | - Format date as "DD / MM / YYYY" with spaces | 🔴 | ⬜ | Dev | - |
| T124 | - Implement DrawString for PayeeName field | 🔴 | ⬜ | Dev | - |
| T125 | - Implement DrawString for Amount field | 🔴 | ⬜ | Dev | - |
| T126 | - Add # guards to amount display | 🔴 | ⬜ | Dev | - |
| T127 | - Implement DrawString for AmountInWords field | 🔴 | ⬜ | Dev | - |
| T128 | - Implement DrawString for Location field | 🔴 | ⬜ | Dev | - |
| T129 | - Handle test print mode (draw rectangles instead) | 🔴 | ⬜ | Dev | - |
| T130 | - Implement error handling (printer offline, etc.) | 🔴 | ⬜ | Dev | - |
| T131 | - Implement ValidatePrinterStatus() method | 🔴 | ⬜ | Dev | - |
| T132 | Create PrinterService tests (mock tests) | 🟡 | ⬜ | Dev | 2h |

**Sprint 3.2 Total:** ~10.5 hours

---

### Phase 3.3: Coordinate System & Calibration Logic

| ID | Task | Priority | Status | Assignee | Est. |
|--- |------|----------|--------|----------|------|
| T133 | Define base coordinates for all check fields | 🔴 | ⬜ | Dev | 2h |
| T134 | - Research standard check dimensions | 🔴 | ⬜ | Dev | - |
| T135 | - Define BaseDateX, BaseDateY | 🔴 | ⬜ | Dev | - |
| T136 | - Define BasePayeeX, BasePayeeY | 🔴 | ⬜ | Dev | - |
| T137 | - Define BaseAmountX, BaseAmountY | 🔴 | ⬜ | Dev | - |
| T138 | - Define BaseAmountInWordsX, BaseAmountInWordsY | 🔴 | ⬜ | Dev | - |
| T139 | - Define BaseLocationX, BaseLocationY | 🔴 | ⬜ | Dev | - |
| T140 | Implement coordinate calculation with offset | 🔴 | ⬜ | Dev | 1h |
| T141 | - ActualX = BaseX + OffsetX | 🔴 | ⬜ | Dev | - |
| T142 | - ActualY = BaseY + OffsetY | 🔴 | ⬜ | Dev | - |
| T143 | - Convert mm to pixels using UnitConverter | 🔴 | ⬜ | Dev | - |

**Sprint 3.3 Total:** ~3 hours

---

### Phase 3.4: App.xaml Configuration

| ID | Task | Priority | Status | Assignee | Est. |
|--- |------|----------|--------|----------|------|
| T144 | Configure Dependency Injection in App.xaml.cs | 🔴 | ⬜ | Dev | 2h |
| T145 | - Register ISettingsService | 🔴 | ⬜ | Dev | - |
| T146 | - Register IPrinterService | 🔴 | ⬜ | Dev | - |
| T147 | - Register INumberToTextConverter | 🔴 | ⬜ | Dev | - |
| T148 | - Register ViewModels | 🔴 | ⬜ | Dev | - |
| T149 | Initialize settings on app startup | 🔴 | ⬜ | Dev | 1h |
| T150 | - Load settings from JSON | 🔴 | ⬜ | Dev | - |
| T151 | - Apply to MainViewModel | 🔴 | ⬜ | Dev | - |

**Sprint 3.4 Total:** ~3 hours

---

**SPRINT 3 TOTAL: ~22 hours**

---

## SPRINT 4 - Testing, Polish & Deployment (Week 4)

### Phase 4.1: Settings Window

| ID | Task | Priority | Status | Assignee | Est. |
|--- |------|----------|--------|----------|------|
| T152 | Create `SettingsViewModel.cs` | 🟡 | ⬜ | Dev | 2h |
| T153 | - Add DefaultLocation property | 🟡 | ⬜ | Dev | - |
| T154 | - Add AutoUpperCase property | 🟡 | ⬜ | Dev | - |
| T155 | - Add FontFamily property | 🟡 | ⬜ | Dev | - |
| T156 | - Add FontSize property | 🟡 | ⬜ | Dev | - |
| T157 | - Add SaveCommand | 🟡 | ⬜ | Dev | - |
| T158 | - Add CancelCommand | 🟡 | ⬜ | Dev | - |
| T159 | Create SettingsWindow.xaml | 🟡 | ⬜ | Dev | 2h |
| T160 | - Design form layout | 🟡 | ⬜ | Dev | - |
| T161 | - Add controls for all settings | 🟡 | ⬜ | Dev | - |
| T162 | - Setup data binding | 🟡 | ⬜ | Dev | - |

**Sprint 4.1 Total:** ~4 hours

---

### Phase 4.2: Input Validation & Error Handling

| ID | Task | Priority | Status | Assignee | Est. |
|--- |------|----------|--------|----------|------|
| T163 | Implement amount input masking | 🔴 | ⬜ | Dev | 2h |
| T164 | - Only allow digits and comma | 🔴 | ⬜ | Dev | - |
| T165 | - Auto-format with thousand separators | 🔴 | ⬜ | Dev | - |
| T166 | - Validate range (0.01 - 999,999,999.99) | 🔴 | ⬜ | Dev | - |
| T167 | Implement date validation | 🔴 | ⬜ | Dev | 1h |
| T168 | - Prevent invalid dates | 🔴 | ⬜ | Dev | - |
| T169 | - Format as required | 🔴 | ⬜ | Dev | - |
| T170 | Implement payee name validation | 🟡 | ⬜ | Dev | 1h |
| T171 | - Max length check | 🟡 | ⬜ | Dev | - |
| T172 | - Required field check | 🟡 | ⬜ | Dev | - |
| T173 | Add validation error messages to UI | 🔴 | ⬜ | Dev | 1h |
| T174 | - Show validation errors near fields | 🔴 | ⬜ | Dev | - |
| T175 | - Disable Print button when invalid | 🔴 | ⬜ | Dev | - |
| T176 | Implement printer error handling | 🔴 | ⬜ | Dev | 2h |
| T177 | - Show user-friendly error dialogs | 🔴 | ⬜ | Dev | - |
| T178 | - Handle printer offline scenario | 🔴 | ⬜ | Dev | - |
| T179 | - Handle no printer installed scenario | 🔴 | ⬜ | Dev | - |

**Sprint 4.2 Total:** ~7 hours

---

### Phase 4.3: UI Polish & UX Enhancements

| ID | Task | Priority | Status | Assignee | Est. |
|--- |------|----------|--------|----------|------|
| T180 | Apply consistent styling (colors, fonts) | 🟡 | ⬜ | Dev | 2h |
| T181 | Add application icon | 🟡 | ⬜ | Dev | 0.5h |
| T182 | Implement keyboard shortcuts | 🟡 | ⬜ | Dev | 1h |
| T183 | - Tab navigation between fields | 🟡 | ⬜ | Dev | - |
| T184 | - Ctrl+P for Print | 🟡 | ⬜ | Dev | - |
| T185 | - Ctrl+S for Save Settings | 🟡 | ⬜ | Dev | - |
| T186 | - Esc to close dialogs | 🟡 | ⬜ | Dev | - |
| T187 | Add tooltips to controls | 🟡 | ⬜ | Dev | 1h |
| T188 | Improve slider visual design | 🟡 | ⬜ | Dev | 1h |
| T189 | Add fade-in animation for preview updates | 🟢 | ⬜ | Dev | 1h |
| T190 | Implement "Save successful" feedback | 🟡 | ⬜ | Dev | 0.5h |

**Sprint 4.3 Total:** ~7 hours

---

### Phase 4.4: Comprehensive Testing

| ID | Task | Priority | Status | Assignee | Est. |
|--- |------|----------|--------|----------|------|
| T191 | Complete all unit tests | 🔴 | ⬜ | QA | 4h |
| T192 | - Ensure >70% code coverage | 🔴 | ⬜ | QA | - |
| T193 | Manual testing: Number to text conversion | 🔴 | ⬜ | QA | 2h |
| T194 | - Test all test cases from PRD | 🔴 | ⬜ | QA | - |
| T195 | Manual testing: Calibration accuracy | 🔴 | ⬜ | QA | 3h |
| T196 | - Test with real printer | 🔴 | ⬜ | QA | - |
| T197 | - Measure actual vs expected positions | 🔴 | ⬜ | QA | - |
| T198 | - Test different offset values | 🔴 | ⬜ | QA | - |
| T199 | Manual testing: Print output quality | 🔴 | ⬜ | QA | 2h |
| T200 | - Test on different printers | 🔴 | ⬜ | QA | - |
| T201 | - Verify font rendering | 🔴 | ⬜ | QA | - |
| T202 | - Check alignment with real checks | 🔴 | ⬜ | QA | - |
| T203 | Manual testing: Settings persistence | 🔴 | ⬜ | QA | 1h |
| T204 | - Close and reopen app, verify settings kept | 🔴 | ⬜ | QA | - |
| T205 | Manual testing: Error scenarios | 🔴 | ⬜ | QA | 2h |
| T206 | - Test with no printer | 🔴 | ⬜ | QA | - |
| T207 | - Test with invalid inputs | 🔴 | ⬜ | QA | - |
| T208 | - Test with corrupted settings file | 🔴 | ⬜ | QA | - |
| T209 | Performance testing | 🟡 | ⬜ | QA | 1h |
| T210 | - Verify number conversion < 100ms | 🟡 | ⬜ | QA | - |
| T211 | - Verify preview update < 50ms | 🟡 | ⬜ | QA | - |
| T212 | Usability testing | 🟡 | ⬜ | QA | 2h |
| T213 | - Test with real users | 🟡 | ⬜ | QA | - |
| T214 | - Gather feedback | 🟡 | ⬜ | QA | - |

**Sprint 4.4 Total:** ~17 hours

---

### Phase 4.5: Documentation

| ID | Task | Priority | Status | Assignee | Est. |
|--- |------|----------|--------|----------|------|
| T215 | Write README.md | 🟡 | ⬜ | Dev | 1h |
| T216 | - Project description | 🟡 | ⬜ | Dev | - |
| T217 | - Installation instructions | 🟡 | ⬜ | Dev | - |
| T218 | - Usage guide | 🟡 | ⬜ | Dev | - |
| T219 | Write USER_GUIDE.md | 🟡 | ⬜ | Dev | 2h |
| T220 | - How to perform initial calibration | 🟡 | ⬜ | Dev | - |
| T221 | - How to print a check | 🟡 | ⬜ | Dev | - |
| T222 | - Troubleshooting section | 🟡 | ⬜ | Dev | - |
| T223 | Write DEVELOPER_GUIDE.md | 🟢 | ⬜ | Dev | 2h |
| T224 | - Architecture overview | 🟢 | ⬜ | Dev | - |
| T225 | - How to build project | 🟢 | ⬜ | Dev | - |
| T226 | - How to run tests | 🟢 | ⬜ | Dev | - |
| T227 | Add inline code comments | 🟡 | ⬜ | Dev | 2h |
| T228 | - Document complex algorithms | 🟡 | ⬜ | Dev | - |
| T229 | - Document coordinate system | 🟡 | ⬜ | Dev | - |

**Sprint 4.5 Total:** ~7 hours

---

### Phase 4.6: Deployment & Packaging

| ID | Task | Priority | Status | Assignee | Est. |
|--- |------|----------|--------|----------|------|
| T230 | Configure Release build | 🔴 | ⬜ | Dev | 1h |
| T231 | - Set optimization flags | 🔴 | ⬜ | Dev | - |
| T232 | - Configure output paths | 🔴 | ⬜ | Dev | - |
| T233 | Create portable package (ZIP) | 🔴 | ⬜ | Dev | 1h |
| T234 | - Include all dependencies | 🔴 | ⬜ | Dev | - |
| T235 | - Include README | 🔴 | ⬜ | Dev | - |
| T236 | - Test on clean machine | 🔴 | ⬜ | Dev | - |
| T237 | Create installer (.msi) | 🟡 | ⬜ | Dev | 3h |
| T238 | - Use WiX Toolset or similar | 🟡 | ⬜ | Dev | - |
| T239 | - Configure install paths | 🟡 | ⬜ | Dev | - |
| T240 | - Add desktop shortcut option | 🟡 | ⬜ | Dev | - |
| T241 | - Test installer on clean machine | 🟡 | ⬜ | Dev | - |
| T242 | Create version numbering system | 🟡 | ⬜ | Dev | 0.5h |
| T243 | Create CHANGELOG.md | 🟡 | ⬜ | Dev | 0.5h |

**Sprint 4.6 Total:** ~6 hours

---

**SPRINT 4 TOTAL: ~48 hours** (can be parallelized)

---

## TOTAL PROJECT ESTIMATE

- **Sprint 1:** ~19.5 hours
- **Sprint 2:** ~19 hours
- **Sprint 3:** ~22 hours
- **Sprint 4:** ~48 hours (testing & deployment can overlap)

**Total:** ~108.5 hours (~13.5 days at 8 hours/day)

**Target:** 4 weeks (160 hours) - leaves buffer for unforeseen issues

---

## POST-LAUNCH (Future Enhancements)

### Version 1.1 Tasks

| ID | Task | Priority | Status |
|--- |------|----------|--------|
| T244 | Implement batch check printing | 🟡 | ⬜ |
| T245 | Add print history/log | 🟡 | ⬜ |
| T246 | Export/import settings | 🟡 | ⬜ |
| T247 | Add check templates for different banks | 🟡 | ⬜ |

### Version 2.0 Tasks

| ID | Task | Priority | Status |
|--- |------|----------|--------|
| T248 | Add support for other currencies | 🟢 | ⬜ |
| T249 | Cloud settings sync | 🟢 | ⬜ |
| T250 | Integration with accounting software | 🟢 | ⬜ |
| T251 | Advanced reporting & analytics | 🟢 | ⬜ |

---

## CRITICAL PATH

These tasks are on the critical path and must be completed in order:

1. **T001-T009:** Project setup
2. **T020-T036:** Number to text converter (blocker for all features)
3. **T114-T131:** Printer service (core functionality)
4. **T102-T110:** Settings service (required for calibration persistence)
5. **T050-T064:** MainViewModel (binds everything together)
6. **T065-T083:** Main window UI (user interface)
7. **T193-T202:** Real printer testing (validation)

---

## RISK ITEMS

| Risk | Mitigation Task | Priority |
|------|----------------|----------|
| Printer DPI differences | T195-T198: Extensive printer testing | 🔴 |
| Font rendering issues | T200-T201: Multi-printer testing | 🔴 |
| Coordinate calculation errors | T133-T143: Thorough coordinate system setup | 🔴 |
| Performance issues | T209-T211: Performance testing | 🟡 |

---

## NOTES FOR DEVELOPERS

### Daily Standup Focus
1. What did I complete yesterday?
2. What will I work on today?
3. Any blockers?

### Definition of Done
- ✅ Code written and committed
- ✅ Unit tests written (where applicable)
- ✅ Code reviewed (if team > 1)
- ✅ Manual testing completed
- ✅ Documentation updated
- ✅ No critical bugs

### Testing Checklist
- [ ] Unit tests pass
- [ ] Manual testing completed
- [ ] Edge cases covered
- [ ] Error handling verified
- [ ] Performance acceptable

### Code Review Checklist
- [ ] Follows C# coding conventions
- [ ] MVVM pattern followed
- [ ] Proper error handling
- [ ] Comments where needed
- [ ] No hardcoded values
- [ ] DI used correctly

---

**Last Updated:** 19 Şubat 2026  
**Version:** 1.0.0

---

# Quick Task Filter Views

## This Week (Sprint 1)
- T001-T045 (Project setup + Core logic)

## Next Week (Sprint 2)
- T046-T101 (UI implementation)

## Week 3 (Sprint 3)
- T102-T151 (Services & functionality)

## Week 4 (Sprint 4)
- T152-T243 (Testing & deployment)

## Critical Only (P0)
Filter by 🔴 priority for minimum viable product

## Can Defer (P2-P3)
Filter by 🟢⚪ for post-MVP features

---

**Ready to start development! 🚀**
