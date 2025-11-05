window.Generator = (() => {
  const SIZES = [100, 1000, 10000, 100000];
  const PRETTY = { sorted:'Ordenado', almost:'Medio', reversed:'Inverso' };

  function seededRandom(seed){
    let s = seed >>> 0;
    return () => (s = (1664525 * s + 1013904223) >>> 0, s / 4294967296);
  }

  function makeArray(n, seed=1234) {
    const rand = seededRandom(seed);
    const arr = new Array(n);
    for (let i=0; i<n; i++) arr[i] = Math.floor(rand()*1_000_000);
    return arr;
  }

  function preorder(arr, type) {
    const a = arr.slice();
    if (type === 'sorted') return a.sort((x,y)=>x-y);
    if (type === 'reversed') return a.sort((x,y)=>y-x);
    if (type === 'almost') {
      a.sort((x,y)=>x-y);
      const swaps = Math.max(1, Math.floor(a.length * 0.5));
      const r = seededRandom(2025);
      for (let i=0;i<swaps;i++){
        const i1 = Math.floor(r()*a.length);
        const i2 = Math.floor(r()*a.length);
        const tmp=a[i1]; a[i1]=a[i2]; a[i2]=tmp;
      }
      return a;
    }
    return a;
  }

  return { SIZES, PRETTY, makeArray, preorder };
})();
