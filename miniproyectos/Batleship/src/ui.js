import { SIZE, coordId } from './util.js';

export function renderGrid(el, board, {clickHandler, revealShips=false, isPlayer=false} = {}){
  el.innerHTML='';
  for(let r=0;r<SIZE;r++){
    for(let c=0;c<SIZE;c++){
      const d=document.createElement('div');
      d.className='cell';
      const v=board[r][c];
      if(v==='H') d.classList.add('hit');
      if(v==='M') d.classList.add('miss');
      if((revealShips || isPlayer) && v==='S') d.classList.add('ship');
      d.dataset.pos = coordId(r,c);
      if(clickHandler) d.addEventListener('click', clickHandler);
      el.appendChild(d);
    }
  }
}

export function toast(msg){
  const el=document.getElementById('toast');
  el.textContent = msg;
  el.style.display='block';
  clearTimeout(toast._t);
  toast._t=setTimeout(()=> el.style.display='none', 1200);
}

export function setTurn(text){
  const el=document.getElementById('turnInfo');
  if(el) el.textContent = `Turno: ${text}`;
}

export function showModal(result, stats){
  document.getElementById('gameResult').textContent = result;
  const st = document.getElementById('gameStats');
  if(st) st.textContent = stats;
  document.getElementById('gameModal').style.display='flex';
}
export function hideModal(){
  const m=document.getElementById('gameModal');
  if(m) m.style.display='none';
}
