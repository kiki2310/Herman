(function initUI(){
  // Tamaños
  const sizesBox = document.getElementById('sizesBox');
  for (const n of Generator.SIZES) {
    const el = document.createElement('label');
    el.className = 'pill';
    el.innerHTML = `<input type="checkbox" class="sizeChk" value="${n}" ${n<=100000?'checked':''}/> n=${n}`;
    sizesBox.appendChild(el);
  }

  // Algoritmos
  const box = document.getElementById('algosBox');
  Algorithms.algorithms.forEach(a=>{
    const row = document.createElement('div');
    row.className = 'row';
    row.innerHTML = `
      <label class="pill">
        <input type="checkbox" class="algoChk" value="${a.key}" checked/>
        <span style="display:inline-flex;align-items:center;gap:8px">
          <span style="width:10px;height:10px;border-radius:50%;background:${Algorithms.COLORS[a.key]}"></span>
          ${a.name}
        </span>
      </label>
      ${a.quadratic ? ` <span class="badge">O(n²)</span>` : ''}`;
    box.appendChild(row);
  });

  document.getElementById('runBtn').onclick = async ()=>{
    document.getElementById('log').textContent = '';
    document.getElementById('tables').innerHTML = '';
    const activeSizes = [...document.querySelectorAll('.sizeChk:checked')].map(i=>parseInt(i.value,10));
    const activePre   = [...document.querySelectorAll('.pre:checked')].map(i=>i.value);
    const activeAlgos = [...document.querySelectorAll('.algoChk:checked')].map(i=>i.value);
    const forceQuadratic = document.getElementById('forceQuadratic').checked;
    const reps = Math.max(1, parseInt(document.getElementById('reps').value||'1',10));

    const {results, scenarioOptions} = await Bench.run({
      sizes: activeSizes, pres: activePre, algos: activeAlgos, forceQuadratic, reps
    });

    // llenar selector
    const sel = document.getElementById('scenarioSelect');
    sel.innerHTML = '';
    scenarioOptions.forEach(s=>{
      const opt=document.createElement('option'); opt.value=s.key; opt.textContent=s.label; sel.appendChild(opt);
    });
    if (scenarioOptions.length) sel.value = scenarioOptions[0].key;

    const render = ()=> ChartUI.renderChart(results, sel.value);
    sel.onchange = render; render();

    ChartUI.renderTables(results);
  };

  document.getElementById('clearBtn').onclick = ()=>{
    document.getElementById('log').textContent = '';
    document.getElementById('tables').innerHTML = '';
    document.getElementById('scenarioSelect').innerHTML = '';
    const canvas = document.getElementById('chart');
    const ctx = canvas.getContext('2d'); ctx.clearRect(0,0,canvas.width,canvas.height);
  };
})();
