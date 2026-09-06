@echo off
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Sync-Runer.ps1" -Acao Pull
pause
