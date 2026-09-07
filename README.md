# Projeto Runer

Jogo de corrida incremental para PC/Steam desenvolvido em Unity e C#.

## Conceito
O jogador começa como uma pessoa comum e tenta percorrer distâncias cada vez maiores. Ao final de cada tentativa, recebe pontos para comprar melhorias permanentes que aumentam sua capacidade de chegar mais longe.

A aparência do personagem evolui junto com os upgrades:

**humano comum → corredor → atleta → super-humano → ciborgue → corredor espacial → entidade cósmica**

## Marcos principais de distância
- 10 m
- 100 m
- 1 km
- 10 km
- 100 km
- 1.000 km
- 10.000 km
- ~40.075 km: volta ao mundo
- 384.400 km: escala Terra–Lua
- ~149,6 milhões km: 1 UA / escala Terra–Sol
- Sistema Solar exterior
- Espaço interestelar

## Protótipo 0.1
O protótipo atual já possui:

- corrida automática baseada em velocidade;
- distância acumulada usando `double`, preparada para escalas muito grandes;
- pulo com **Espaço**, **Seta para cima** ou **W**;
- obstáculos gerados automaticamente com espaçamento baseado em distância;
- colisão e encerramento da tentativa;
- pontos ganhos ao fim da corrida;
- pontos e recorde persistentes via `PlayerPrefs`;
- upgrade permanente de velocidade;
- HUD com distância, velocidade, pontos, recorde e próximo marco;
- tela de game over e reinício;
- evolução visual provisória do personagem conforme o nível de velocidade;
- cena de protótipo criada automaticamente em runtime, sem necessidade de prefabs.

## Como o protótipo funciona
`PrototypeBootstrap.cs` monta automaticamente, ao entrar em Play Mode:

1. Game Manager;
2. câmera;
3. iluminação;
4. chão;
5. personagem;
6. gerador de obstáculos;
7. HUD.

Isso permite testar a mecânica antes de produzir os modelos 3D definitivos.

## Controles
- **Espaço / ↑ / W**: pular
- **R**: reiniciar depois do game over
- Botões na tela de game over: comprar upgrade e correr novamente

## Estrutura
- `Assets/Scripts/RunGameManager.cs` — distância, pontos, recorde e upgrades
- `Assets/Scripts/RunnerController.cs` — controle e pulo
- `Assets/Scripts/RunnerCollision.cs` — colisão com obstáculos
- `Assets/Scripts/ObstacleSpawner.cs` — geração de obstáculos
- `Assets/Scripts/WorldScroller.cs` — movimentação do mundo
- `Assets/Scripts/RunHUD.cs` — interface e marcos de distância
- `Assets/Scripts/CharacterEvolution.cs` — evolução visual
- `Assets/Scripts/PrototypeBootstrap.cs` — criação automática do protótipo

## Próximos passos
### 0.2
- substituir a cápsula por personagem humano 3D;
- animação de corrida e pulo;
- cenário modular em movimento;
- mais tipos de obstáculos;
- melhorias de pulo e multiplicador de pontos;
- efeitos visuais de velocidade;
- sistema de marcos com recompensas.

### Futuro
- eras: Terra, atmosfera, espaço, Sistema Solar e interestelar;
- Steam Achievements;
- Steam Cloud;
- leaderboards;
- sons e música;
- opções gráficas;
- save versionado.

## Engine
Recomendação atual: **Unity 6.3 LTS**, C#.

## Versao 0.2 - Parque urbano

Personagem humano 3D estilizado e articulado, com animacao procedural de corrida proporcional a velocidade e pose de salto ligada a fisica. Parque em segmentos reciclados com pista, gramado, arvores, bancos, postes e skyline ao por do sol. Modelos originais gerados em C#, sem assets externos. A primeira barreira tem um intervalo maior para preparar o salto. Controles: Espaco / seta para cima / W; R reinicia apos colisao. Abra uma cena vazia e pressione Play em Unity 6000.3.17f1.
