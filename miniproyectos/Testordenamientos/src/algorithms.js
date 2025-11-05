// Mapa de algoritmos y colores fijos
window.Algorithms = (() => {
  // Paleta fija por algoritmo (barras Chart.js)
  const COLORS = {
    bubble:  '#ef4444',   // rojo
    select:  '#f59e0b',   // ámbar
    insert:  '#10b981',   // verde
    merge:   '#3b82f6',   // azul
    quick:   '#8b5cf6',   // violeta
    heap:    '#22c55e',   // verde claro
    hash:   '#06b6d4',   // cian
    radix:   '#e11d48',   // rosa fuerte
    bucket:  '#0ea5e9'    // azul claro
  };

  // ---- Burbuja
  function burbuja(arr) {
    const a = arr.slice(); let n = a.length, vaso;
    for (let i = 0; i < n - 1; i++) {
      let swapped=false;
      for (let j = 0; j < n - i - 1; j++) {
        if (a[j] > a[j + 1]) { vaso = a[j]; a[j] = a[j + 1]; a[j + 1] = vaso; swapped=true; }
      }
      if(!swapped) break;
    }
    return a;
  }
  // ---- Selección
  function seleccion(arr) {
    const a = arr.slice(); let n=a.length, minIndex, vaso;
    for (let i=0;i<n-1;i++){
      minIndex=i; for (let j=i+1;j<n;j++) if (a[j]<a[minIndex]) minIndex=j;
      if (minIndex!==i){ vaso=a[i]; a[i]=a[minIndex]; a[minIndex]=vaso; }
    }
    return a;
  }
  // ---- Inserción
  function inserccion(arr) {
    const a = arr.slice();
    for (let i=1;i<a.length;i++){
      let v=a[i], j=i-1;
      while(j>=0 && a[j]>v){ a[j+1]=a[j]; j--; }
      a[j+1]=v;
    }
    return a;
  }
  // ---- Heap
  function heapify(a, n, i){
    let largest=i, l=2*i+1, r=2*i+2;
    if (l<n && a[l]>a[largest]) largest=l;
    if (r<n && a[r]>a[largest]) largest=r;
    if (largest!==i){ [a[i],a[largest]]=[a[largest],a[i]]; heapify(a,n,largest); }
  }
  function heapsort(arr){
    const a = arr.slice(); const n=a.length;
    for (let i=Math.floor(n/2)-1;i>=0;i--) heapify(a,n,i);
    for (let i=n-1;i>0;i--){ [a[0],a[i]]=[a[i],a[0]]; heapify(a,i,0); }
    return a;
  }
  // ---- Quick
  function quicksortBase(a, randomPivot=true){
    if (a.length<=1) return a.slice();
    const arr=a.slice();
    const p = randomPivot ? Math.floor(Math.random()*arr.length) : arr.length-1;
    const pivot=arr[p], left=[], right=[];
    for (let i=0;i<arr.length;i++){ if(i===p) continue; (arr[i]<pivot?left:right).push(arr[i]); }
    return [...quicksortBase(left, randomPivot), pivot, ...quicksortBase(right, randomPivot)];
  }
  function quicksort(arr, opts={randomPivot:true}){ return quicksortBase(arr, !!opts.randomPivot); }
  // ---- Merge (estable)
  function merge(arr, l, m, r){
    let n1=m-l+1, n2=r-m;
    let L=new Array(n1), R=new Array(n2);
    for (let i=0;i<n1;i++) L[i]=arr[l+i];
    for (let j=0;j<n2;j++) R[j]=arr[m+1+j];
    let i=0,j=0,k=l;
    while(i<n1 && j<n2) arr[k++]=(L[i]<=R[j])?L[i++]:R[j++];
    while(i<n1) arr[k++]=L[i++];
    while(j<n2) arr[k++]=R[j++];
  }
  function mergeSort(arr){
    const a=arr.slice();
    const rec=(l,r)=>{ if(l>=r) return; const m=l+Math.floor((r-l)/2); rec(l,m); rec(m+1,r); merge(a,l,m,r); };
    rec(0,a.length-1); return a;
  }
  // ---- Shell
  function hashsort(arr){
    const a = arr.slice(); const n = a.length;
    for (let gap=Math.floor(n/2); gap>0; gap=Math.floor(gap/2)){
      for (let j=gap;j<n;j++){
        let v=a[j], k=j;
        while(k>=gap && a[k-gap]>v){ a[k]=a[k-gap]; k-=gap; }
        a[k]=v;
      }
    }
    return a;
  }
  // ---- Radix (enteros ≥ 0)
  function countingSortByExp(a, exp){
    const n=a.length, out=new Array(n), cnt=new Array(10).fill(0);
    for (let i=0;i<n;i++) cnt[Math.floor((a[i]/exp)%10)]++;
    for (let i=1;i<10;i++) cnt[i]+=cnt[i-1];
    for (let i=n-1;i>=0;i--){ const idx=Math.floor((a[i]/exp)%10); out[cnt[idx]-1]=a[i]; cnt[idx]--; }
    return out;
  }
  function radixSort(arr){
    let a=arr.slice(); const max=Math.max(...a); let exp=1;
    while (Math.floor(max/exp)>0){ a=countingSortByExp(a,exp); exp*=10; }
    return a;
  }
  // ---- Bucket (normaliza rango)
  function insertionSort(bukt){
    for (let j=1;j<bukt.length;j++){
      const val=bukt[j]; let k=j-1;
      while(k>=0 && bukt[k]>val){ bukt[k+1]=bukt[k]; k--; }
      bukt[k+1]=val;
    }
    return bukt;
  }
  function bucketSort(inputArr){
    const a=inputArr.slice(); const n=a.length; if(n===0) return a;
    const min=Math.min(...a), max=Math.max(...a), range=(max-min)||1;
    const buckets=Array.from({length:n},()=>[]);
    for (const x of a){ const idx=Math.min(n-1, Math.floor(((x-min)/range)*n)); buckets[idx].push(x); }
    const res=[]; for (const b of buckets){ insertionSort(b); res.push(...b); }
    return res;
  }

  const algorithms = [
    {key:'bubble',   name:'Burbuja ',    fn: burbuja,  quadratic:true},
    {key:'select',   name:'Selección ',  fn: seleccion, quadratic:true},
    {key:'insert',   name:'Inserción',  fn: inserccion, quadratic:true},
    {key:'merge',    name:'Merge Sort',         fn: mergeSort},
    {key:'quick',    name:'Quick Sort',         fn: (a)=>quicksort(a,{randomPivot:document.getElementById('randomPivot')?.checked ?? true}) },
    {key:'heap',     name:'Heap Sort',          fn: heapsort},
    {key:'hash',    name:'Hash Sort',         fn: hashsort},
    {key:'radix',    name:'Radix Sort ',    fn: radixSort},
    {key:'bucket',   name:'Bucket Sort',        fn: bucketSort}
  ];

  return { algorithms, COLORS };
})();
