import { newBoard } from './util.js';
import { renderGrid, toast } from './ui.js';
import { Placement } from './placement.js';

const elPlayer = document.getElementById('playerGrid');
const btnReset = document.getElementById('btnReset');
const btnStart = document.getElementById('btnStart');
const shipPalette = document.getElementById('shipPalette');
const btnRotate = document.getElementById('btnRotate');
const swapModal = document.getElementById('swapModal');
const swapTitle = document.getElementById('swapTitle');
const swapMsg = document.getElementById('swapMsg');
const btnSwapContinue = document.getElementById('btnSwapContinue');

let boards = [newBoard(), newBoard()];
let current = 0;
let phase = 'placement1';
let placement;

function draw() {
  renderGrid(elPlayer, boards[current], { isPlayer: true });
}

function startPlacement(playerIndex) {
  current = playerIndex;
  btnStart.textContent = playerIndex === 0 ? 'Continuar' : 'Abrir batalla';
  placement = new Placement(boards[current], elPlayer, shipPalette, btnRotate, btnStart);
}

function swapTurn() {
  const next = 1 - current;
  swapTitle.textContent = `Entrega la computadora al Jugador ${next + 1}`;
  swapMsg.textContent = next === 1 ? 'Jugador 2: coloca tu formación.' : 'Abriré la batalla en una pestaña nueva.';
  swapModal.style.display = 'flex';
}

btnReset.addEventListener('click', () => {
  boards = [newBoard(), newBoard()];
  phase = 'placement1';
  localStorage.removeItem('battleship:boards');
  swapModal.style.display = 'none';
  startPlacement(0);
  draw();
});

btnStart.addEventListener('click', () => {
  if (placement && placement.remaining.length === 0) {
    swapTurn();
  } else {
    toast('Completa tu formación primero.');
  }
});

btnSwapContinue.addEventListener('click', () => {
  swapModal.style.display = 'none';
  if (phase === 'placement1') {
    phase = 'placement2';
    startPlacement(1);
    draw();
  } else if (phase === 'placement2') {
    try {
      localStorage.setItem('battleship:boards', JSON.stringify(boards));
    } catch (e) {
      console.warn('No se pudo guardar en localStorage', e);
    }
    window.open('./battle.html', '_blank');
  }
});

startPlacement(0);
draw();
