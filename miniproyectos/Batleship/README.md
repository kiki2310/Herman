# Battleship Mini — Hotseat con pestaña de batalla

## Estructura
- `index_hotseat.html`: pantalla de **formaciones 1 vs 1**. Al terminar, guarda en `localStorage` y abre `battle.html` en otra pestaña.
- `battle.html`: **tablero de batalla** en nueva pestaña, con alternancia de turnos y pantalla de entrega.
- `index.html`: demo/placeholder para modo CPU (opcional).

## Ejecutar
Sirve por HTTP (módulos ES):
- Python: `python -m http.server 8080` -> abre `http://localhost:8080/index_hotseat.html`
- VSCode Live Server: click derecho -> Open with Live Server.

## Flujo
1. Jugador 1 coloca su flota -> Continuar -> pantalla de entrega.
2. Jugador 2 coloca su flota -> Abrir batalla -> se abre `battle.html`.
3. En `battle.html` se alternan turnos con modal de entrega. Fin cuando un tablero queda sin barcos.
