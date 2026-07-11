1. POST /api/simulaciones con { capitalInicial: 10000, tasaAnual: 0.05, plazoAnios: 3 }
2. GET /api/simulaciones — lista todas
3. GET /api/simulaciones/1 — resumen general
4. GET /api/simulaciones/1/proyeccion-mensual — 36 registros
5. GET /api/simulaciones/1/proyeccion-anual — 3 registros
6. GET /api/simulaciones/999 — debe retornar 404
7. POST con valores negativos — debe retornar 400