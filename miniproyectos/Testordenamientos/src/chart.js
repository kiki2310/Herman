window.ChartUI = (() => {
  let chart;

  function scenarioLabelFromKey(key){
    const [n, pre] = key.split('-');
    return `n=${n} • ${Generator.PRETTY[pre]||pre}`;
  }

  function renderChart(results, scenarioKey){
    const rows = (results[scenarioKey] || []).filter(r=>!r.skipped && Number.isFinite(r.ms));
    const labels = rows.map(r=>r.algo);
    const data = rows.map(r=>r.ms);
    const bg = rows.map(r=>Algorithms.COLORS[r.key] || '#94a3b8');

    if (chart) chart.destroy();
    const ctx = document.getElementById('chart').getContext('2d');
    chart = new Chart(ctx, {
      type:'bar',
      data:{ labels, datasets:[{ label:`Tiempo (ms) • ${scenarioKey}`, data, backgroundColor:bg }] },
      options:{
        responsive:true,
        plugins:{ legend:{display:false}, tooltip:{mode:'index', intersect:false} },
        scales:{ y:{beginAtZero:true, title:{display:true, text:'ms'}} }
      }
    });
  }

  function renderTables(results){
    const tables = document.getElementById('tables');
    tables.innerHTML = '';
    for (const s of Object.keys(results)) {
      const rows = results[s].slice().filter(r=>Number.isFinite(r.ms)).sort((a,b)=>a.ms-b.ms);
      const div = document.createElement('div');
      div.className = 'card';
      div.style.padding='0';
      div.innerHTML = `
        <div style="padding:12px 14px; border-bottom:1px solid var(--border); display:flex; justify-content:space-between; align-items:center">
          <div><strong>${scenarioLabelFromKey(s)}</strong></div>
          <div class="small">${rows.length} algoritmos</div>
        </div>
        <div style="padding:10px 12px; overflow:auto">
          <table>
            <thead><tr><th>#</th><th>Algoritmo</th><th>Tiempo (ms)</th><th>OK</th></tr></thead>
            <tbody>
              ${rows.map((r,i)=>`<tr>
                <td>${i+1}</td>
                <td><span style="display:inline-block;width:10px;height:10px;border-radius:50%;background:${Algorithms.COLORS[r.key]};margin-right:8px;"></span>${r.algo}</td>
                <td>${r.ms.toFixed(2)}</td>
                <td>${r.ok?'<span class="ok">✔</span>':'<span class="warn">✖</span>'}</td>
              </tr>`).join('')}
            </tbody>
          </table>
        </div>`;
      tables.appendChild(div);
    }
  }

  return { renderChart, renderTables };
})();
