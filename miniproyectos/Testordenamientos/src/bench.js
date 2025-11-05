window.Bench = (() => {
  function log(msg){
    const el = document.getElementById('log');
    el.textContent += msg + "\n";
    el.scrollTop = el.scrollHeight;
  }

  function timeAlgorithm(fn, data) {
    const t0 = performance.now();
    const out = fn(data);
    const t1 = performance.now();
    for (let i=1;i<out.length;i++) if(out[i] < out[i-1]) return {ms:t1-t0, ok:false};
    return {ms:t1-t0, ok:true};
  }

  async function run({sizes, pres, algos, forceQuadratic, reps}) {
    const results = {}; // scenario -> array de {algo, ms, ok, skipped}
    const scenarioOptions = [];

    for (const n of sizes) {
      const base = Generator.makeArray(n, 1234);
      for (const pre of pres) {
        const scenarioKey = `${n}-${pre}`;
        scenarioOptions.push({key:scenarioKey, label:`n=${n} • ${Generator.PRETTY[pre]}`});
        results[scenarioKey] = [];
        const data = Generator.preorder(base, pre);

        log(`Preparado escenario n=${n}, ${Generator.PRETTY[pre]}…`);

        for (const akey of algos) {
          const alg = Algorithms.algorithms.find(x=>x.key===akey);
          if (!alg) continue;

          if (!forceQuadratic && alg.quadratic && n >= 100000){
            log(`  ⏭️  ${alg.name} omitido en n=${n} (O(n²))`);
            results[scenarioKey].push({algo:alg.name, ms:NaN, ok:true, skipped:true, key:akey});
            continue;
          }

          await new Promise(r=>setTimeout(r)); // cede UI
          let best = Infinity, okAll=true;
          for (let i=0;i<reps;i++){
            const {ms, ok} = timeAlgorithm(alg.fn, data);
            okAll = okAll && ok;
            if (ms < best) best = ms;
          }
          results[scenarioKey].push({algo:alg.name, ms:best, ok:okAll, key:akey});
          log(`  ✅ ${alg.name}: ${best.toFixed(2)} ms ${okAll?'':'(falló verificación)'}`);
        }
      }
    }
    return {results, scenarioOptions};
  }

  return { run, log };
})();
