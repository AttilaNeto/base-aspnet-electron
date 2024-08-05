const { exec } = require('child_process');
const path = require('path');

// Define o caminho para o diretório do frontend
const clientAppPath = path.join(__dirname, 'clientApp');

// Inicia o frontend
const startFrontend = exec('npm run dev', { cwd: clientAppPath });

startFrontend.stdout.on('data', (data) => {
    console.log(`Frontend: ${data}`);
});

startFrontend.stderr.on('data', (data) => {
    console.error(`Frontend Error: ${data}`);
});

startFrontend.on('close', (code) => {
    console.log(`Frontend process exited with code ${code}`);
});

setTimeout(() => {
    // Inicia o backend com Electron
    const startBackend = exec('electronize start', { cwd: __dirname });

    startBackend.stdout.on('data', (data) => {
        console.log(`Backend: ${data}`);
    });

    startBackend.stderr.on('data', (data) => {
        console.error(`Backend Error: ${data}`);
    });

    startBackend.on('close', (code) => {
        console.log(`Backend process exited with code ${code}`);
        // Encerra o frontend quando o backend for fechado
        startFrontend.kill();
        process.exit();
    });

    startBackend.on('error', (err) => {
        console.error(`Failed to start backend: ${err}`);
        // Encerra o frontend se houver erro ao iniciar o backend
        startFrontend.kill('SIGTERM');
    });
}, 5000); // Ajuste o tempo conforme necessário