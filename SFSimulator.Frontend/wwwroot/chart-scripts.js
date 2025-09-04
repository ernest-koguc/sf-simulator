Chart.register(ChartDataLabels);
const charts = {};

window.initBarChart = function (canvasId, data, title) {
    const chartData = {
        datasets: data.map(d => ({ data: [d.data], label: d.label })),
    }

    if (charts[canvasId] !== undefined) {
        charts[canvasId].destroy();
    }

    const ctx = document.getElementById(canvasId);

    const chart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: [''],
            datasets: chartData.datasets,
        },
        options: {
            indexAxis: window.innerWidth <= 450 ? 'y' : 'x',
            aspectRatio: window.innerWidth <= 450 ? 0.5 : 1,
            scales: {
                
                y: {
                    display: window.innerWidth <= 450 ? false : true,
                    beginAtZero: true,
                    offset: true,
                    grid: {
                        display: true,
                        color: 'rgba(255,255,255,0.2)'
                    },
                    ticks:
                    {
                        callback: v => format(v),
                        autoSkip: false,
                        color: 'white'
                    }
                },
                x: {
                    display: window.innerWidth <= 450 ? true : false,
                    beginAtZero: true,
                    offset: true,
                    grid: {
                        display: true,
                        color: 'rgba(255,255,255,0.2)'
                    },
                    ticks:
                    {
                        callback: v => format(v),
                        autoSkip: false,
                        color: 'white'
                    }
                }
            },
            plugins: {
                legend: {
                    labels: {
                        font: {
                            family: "'Poppins', sans-serif",
                            weight: "bold"
                        },
                        color: 'white'
                    }
                },
                datalabels: {
                  display: window.innerWidth <= 450 ? false : true,
                  anchor: 'end',
                  color: 'white',
                  align: 'top',
                  formatter: (v, _) => format(v, 1),
                  font: {
                    weight: 'bold',
                    size: 12
                  }
                },
                title: {
                    display: true,
                    text: title,
                    color: 'white',
                    font: {
                        family: "'Poppins', sans-serif",
                        weight: "bold",
                        size: 20
                    },
                }
            }
        }
    });
    charts[canvasId] = chart;
}

window.initLineChart = function (canvasId, data, title) {
    const chartData = {
        labels: data.map(d => d.label),
        datasets: [{
            data: data.map(d => d.data),
        }]
    }
    if (charts[canvasId] !== undefined) {
        charts[canvasId].destroy();
    }

    const ctx = document.getElementById(canvasId);

    const chart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: chartData.labels,
            datasets: chartData.datasets,
        },
        options: {
            aspectRatio: 2,
            pointStyle: false,
            scales: {
                y: {
                    display: true,
                    offset: true,
                    grid: {
                        display: true,
                        color: 'rgba(255,255,255,0.2)',
                        offset: true,
                    },
                    ticks:
                    {
                        autoSkip: false,
                        color: 'white',
                        padding: 0,

                    },
                },
                x: {
                    display: false,
                    offset: true,
                    grid: {
                        display: true,
                        color: 'rgba(255,255,255,0.2)',
                        offset: true,
                    },
                    ticks:
                    {
                        autoSkip: false,
                        color: 'white',
                        padding: 0,
                    },
                    title: {
                        padding: 0,
                    },
                }
            },
            plugins: {
                legend: {
                    display: false,
                },
                datalabels: {
                  display: false,
                },
                title: {
                    display: true,
                    text: title,
                    color: 'white',
                    font: {
                        family: "'Poppins', sans-serif",
                        weight: "bold",
                        size: 20
                    },
                },
            }
        }
    });
    charts[canvasId] = chart;
}

window.destroyChart = function (canvasId) {
    charts[canvasId]?.destroy();
    delete charts[canvasId];
}


function format(value, normalizeTo) {
    if (typeof value == 'string')
      value = parseInt(value);

    if (value >= 1000000000) {
      var formatedValue = (value / 1000000000).toFixed(0) + "B";
      return formatedValue;
    }

    if (value >= 1000000) {
      var formatedValue = (value / 1000000).toFixed(0) + "M";
      return formatedValue;
    }

    if (value >= 1000) {
      var formatedValue = (value/1000).toFixed(0) + "K";
      return formatedValue;
    }

      return value.toFixed(normalizeTo);
  }
