import { newBoard, allSunk, countHits } from './util.js';
import { renderGrid, setTurn, toast, showModal, hideModal } from './ui.js';

const elPlayer = document.getElementById('playerGrid');
const elEnemy  = document.getElementById('enemyGrid');
const swapModal = document.getElementById('swapModal');
const btnSwapContinue = document.getElementById('btnSwapContinue');
const btnReset = document.getElementById('btnReset');

let boards = [newBoard(), newBoard()];
let shots = [new Set(), new Set()];
let current = 0;

// Cargar formaciones
(function loadFormations(){
  const raw = localStorage.getItem('battleship:boards');
  try{
    const parsed = JSON.parse(raw);
    if(Array.isArray(parsed) && parsed.length===2){
      boards = parsed;
    } else {
      toast('No hay formaciones guardadas. Regresa a index_hotseat.');
    }
  } catch { toast('Error cargando formaciones.'); }
})();

function draw(){
  renderGrid(elPlayer, boards[current], { isPlayer:true });
  renderGrid(elEnemy, boards[1-current], { clickHandler:onShoot });
  setTurn(`Jugador ${current+1}`);
}
function onShoot(e){
  const cell = e.target.closest('.cell'); if(!cell) return;
  const [r,c] = cell.dataset.pos.split('-').map(Number);
  const enemy = 1-current;
  const key = `${r}-${c}`;
  if(shots[current].has(key)){ toast('Ya disparaste ahí'); return; }
  shots[current].add(key);
  const v = boards[enemy][r][c];
  if(v==='S'){ boards[enemy][r][c]='H'; toast('¡Tocado!'); }
  else if(v===0){ boards[enemy][r][c]='M'; toast('Agua'); }
  renderGrid(elEnemy, boards[enemy], { clickHandler:onShoot });
  if(allSunk(boards[enemy])){
    showModal(`¡Gana el Jugador ${current+1}!`, `Aciertos J1: ${countHits(boards[1])} · Aciertos J2: ${countHits(boards[0])}`);
    return;
  }
  swapModal.style.display='flex';
}
btnSwapContinue.addEventListener('click', ()=>{
  swapModal.style.display='none';
  current = 1-current;
  draw();
});
btnReset.addEventListener('click', ()=>{
  window.location.href = './index_hotseat.html';
});

hideModal();
draw();
