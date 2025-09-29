// Simple placeholder for CPU mode (optional to use)
import { newBoard, allSunk, countHits } from './util.js';
import { renderGrid, setTurn, toast, showModal, hideModal } from './ui.js';

const elPlayer=document.getElementById('playerGrid');
const elCPU=document.getElementById('cpuGrid');
const btnReset=document.getElementById('btnReset');
const btnReveal=document.getElementById('btnReveal');
const btnPlayAgain=document.getElementById('btnPlayAgain');

let playerBoard=newBoard();
let cpuBoard=newBoard();
let playerShots=new Set();
let revealCPU=false;

function draw(){
  renderGrid(elPlayer,playerBoard,{isPlayer:true});
  renderGrid(elCPU,cpuBoard,{revealShips:revealCPU,clickHandler:onShoot});
}
function onShoot(e){
  const [r,c]=e.target.dataset.pos.split('-').map(Number);
  const key=`${r}-${c}`;
  if(playerShots.has(key)) return;
  playerShots.add(key);
  if(cpuBoard[r][c]==='S'){ cpuBoard[r][c]='H'; toast('¡Tocado!'); }
  else { cpuBoard[r][c]='M'; toast('Agua'); }
  draw();
  if(allSunk(cpuBoard)){ showModal('¡Ganaste!',`Aciertos: ${countHits(cpuBoard)}`); }
}
btnReset.addEventListener('click',()=>{playerBoard=newBoard();cpuBoard=newBoard();playerShots=new Set();hideModal();draw();});
btnReveal.addEventListener('click',()=>{revealCPU=!revealCPU;draw();});
btnPlayAgain.addEventListener('click',()=>{playerBoard=newBoard();cpuBoard=newBoard();playerShots=new Set();hideModal();draw();});
draw();
