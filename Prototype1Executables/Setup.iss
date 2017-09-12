; Script generated for the MonoGame Ruge Project Deploy Tool
; Don't run this script in Inno Setup; use the Ruge Deploy Tool

#define MyAppName "Lost"
#define MyAppVersion "1.0"
#define MyAppPublisher "Four Frogs"
#define MyAppExeName "Monodemo.exe"
#define MyReleaseDir "C:\Users\u0774750\Documents\GitHub\Four-Frogs\Monodemo\Monodemo\bin\Windows\x86\Release"
#define MyDeployDir "C:\Users\u0774750\Desktop\Prototype1Executable"
#define MyNamespace "Monodemo"
#define MyGuid "29dee178-83a2-4d54-b5a3-89d07853a4f7"
#define MyIcon "C:\Users\u0774750\Downloads\topFrog_0000_09-copy.png.ico"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{{#MyGuid}}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={pf}\{#MyAppName}
DisableProgramGroupPage=yes
;LicenseFile={#MyReleaseDir}\eula.txt
;InfoBeforeFile={#MyReleaseDir}\changelog.txt
OutputDir={#MyDeployDir} 
OutputBaseFilename={#MyNamespace}.{#MyAppVersion}.Windows.Setup
Compression=lzma
SolidCompression=yes
SetupIconFile={#MyIcon}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#MyReleaseDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{commonprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

