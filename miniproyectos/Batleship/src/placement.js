import { SHIPS, placeShip } from './util.js';
import { renderGrid, toast } from './ui.js';

export class Placement {
  constructor(playerBoard, gridEl, paletteEl, rotateBtn, startBtn){
    this.board = playerBoard;
    this.gridEl = gridEl;
    this.paletteEl = paletteEl;
    this.rotateBtn = rotateBtn;
    this.startBtn = startBtn;
    this.horiz = true;
    this.remaining = SHIPS.slice(); // copia
    this.activeLen = null;
    this.init();
  }
  init(){
    this.renderPalette();
    this.rotateBtn.addEventListener('click', () => this.toggleOrient());
    window.addEventListener('keydown', (e)=> { if(e.key.toLowerCase()==='r') this.toggleOrient(); });
    this.gridEl.addEventListener('click', (e) => this.handleClick(e));
    renderGrid(this.gridEl, this.board, {isPlayer:true});
  }
  toggleOrient(){
    this.horiz = !this.horiz;
    this.rotateBtn.textContent = this.horiz ? 'Horizontal' : 'Vertical';
  }
  renderPalette(){
    this.paletteEl.innerHTML = '';
    this.remaining.forEach((len) => {
      const chip = document.createElement('button');
      chip.className='ship-chip';
      chip.textContent = `Barco ${len}`;
      chip.addEventListener('click', () => {
        this.activeLen = len;
        [...this.paletteEl.children].forEach(ch => ch.classList.remove('active'));
        chip.classList.add('active');
      });
      this.paletteEl.appendChild(chip);
    });
  }
  handleClick(e){
    const cell = e.target.closest('.cell');
    if(!cell) return;
    if(!this.activeLen){ toast('Elige un barco en la paleta.'); return; }
    const [r,c] = cell.dataset.pos.split('-').map(Number);
    if(placeShip(this.board, r, c, this.activeLen, this.horiz)){
      // quitar una instancia de ese len de remaining
      const idx = this.remaining.indexOf(this.activeLen);
      if(idx>=0) this.remaining.splice(idx,1);
      this.activeLen = null;
      this.renderPalette();
      renderGrid(this.gridEl, this.board, {isPlayer:true});
      if(this.remaining.length===0){
        this.startBtn.disabled = false;
        toast('¡Formación completa! Listo para pasar.');
      }
    } else {
      toast('No cabe o se sobrepone. Prueba otra celda/orientación.');
    }
  }
  reset(board){
    this.board = board;
    this.horiz = true;
    this.remaining = SHIPS.slice();
    this.activeLen = null;
    this.rotateBtn.textContent = 'Horizontal';
    this.startBtn.disabled = true;
    this.renderPalette();
    renderGrid(this.gridEl, this.board, {isPlayer:true});
  }
}
