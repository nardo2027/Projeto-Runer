param([ValidateSet('Status','Pull','Enviar')][string]$Acao='Status')
$ErrorActionPreference='Stop'
$gitExe='C:\Program Files\Git\cmd\git.exe'
Set-Location $PSScriptRoot
function GitRun { & $gitExe @args; if ($LASTEXITCODE -ne 0) { throw 'Operacao Git interrompida. Nenhum reset ou envio forcado foi feito.' } }
try {
 if ((& $gitExe remote get-url origin) -ne 'https://github.com/nardo2027/Projeto-Runer.git') { throw 'Origin inesperado.' }
 if ((& $gitExe branch --show-current) -ne 'main') { throw 'Selecione main antes de sincronizar.' }
 if ($Acao -eq 'Status') { GitRun status; exit }
 if (Test-Path 'Temp/UnityLockfile') { throw 'Salve e feche o Unity antes de sincronizar.' }
 if ($Acao -eq 'Pull') {
  if (& $gitExe status --porcelain) { throw 'Ha alteracoes locais. Envie ou revise antes de atualizar.' }
  GitRun pull --ff-only origin main
 } else {
  if (-not (& $gitExe config user.name)) { $nome=Read-Host 'Seu nome para os commits'; if (-not $nome.Trim()) { throw 'Nome obrigatorio' }; GitRun config --local user.name $nome }
  if (-not (& $gitExe config user.email)) { $email=Read-Host 'Seu email GitHub (ou noreply)'; if (-not $email.Trim()) { throw 'Email obrigatorio' }; GitRun config --local user.email $email }
  GitRun fetch origin
  & $gitExe merge-base --is-ancestor origin/main HEAD
  if ($LASTEXITCODE -ne 0) { throw 'GitHub tem commits que faltam aqui. Pare e solicite revisao da sincronizacao.' }
  GitRun status --short
  GitRun diff --stat
  GitRun diff --cached --stat
  if (& $gitExe status --porcelain) {
   if ((Read-Host 'Revise os arquivos acima. Digite ENVIAR para incluir todas as alteracoes') -cne 'ENVIAR') { throw 'Cancelado' }
   $mensagem=Read-Host 'Descreva a alteracao'; if (-not $mensagem.Trim()) { throw 'Mensagem obrigatoria' }
   GitRun add --all
   GitRun commit -m $mensagem
  }
  GitRun push origin main
 }
} catch { Write-Host $_ -ForegroundColor Red; exit 1 }
