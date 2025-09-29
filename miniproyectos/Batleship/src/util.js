export const SIZE = 10;
export const SHIPS = [5,4,3,3,2];

export function newBoard(){
  return Array.from({length: SIZE}, () => Array(SIZE).fill(0));
}
export function cloneBoard(b){ return b.map(r => r.slice()); }
export function inBounds(r,c){ return r>=0 && r<SIZE && c>=0 && c<SIZE; }

export function placeShip(board, r, c, len, horiz){
  if(horiz){
    if(c+len>SIZE) return false;
    for(let i=0;i<len;i++){ if(board[r][c+i]!==0) return false; }
    for(let i=0;i<len;i++){ board[r][c+i]='S'; }
  } else {
    if(r+len>SIZE) return false;
    for(let i=0;i<len;i++){ if(board[r+i][c]!==0) return false; }
    for(let i=0;i<len;i++){ board[r+i][c]='S'; }
  }
  return true;
}

export function allSunk(board){
  for(let r=0;r<SIZE;r++){ for(let c=0;c<SIZE;c++){ if(board[r][c]==='S') return false; } }
  return true;
}
export function countHits(board){
  let n=0; for(let r=0;r<SIZE;r++){ for(let c=0;c<SIZE;c++){ if(board[r][c]==='H') n++; } } return n;
}

export function coordId(r,c){ return `${r}-${c}`; }
export function parseCoord(id){ const [r,c]=id.split('-').map(Number); return {r,c}; }
