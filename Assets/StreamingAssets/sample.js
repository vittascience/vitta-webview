window.addEventListener(
    'load',
    function() {
        document.body.style.backgroundColor = 'white';
        window.setTimeout(
            function() {
                document.body.style.backgroundColor = '#ABEBC6';
                var msg = document.getElementById("msg");
                msg.textContent = '(NOTE : la couleur de fond a été changée par sample.js, pour vérifier si le code js externe fonctionne)';
            },
            3000);
    });
