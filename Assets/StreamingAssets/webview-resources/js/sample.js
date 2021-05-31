window.addEventListener(
    'load',
    function() {
        window.setTimeout(
            function() {
                document.body.style.backgroundColor = '#fdfdfd';
                var msg = document.getElementById("msg");
                msg.textContent += '(NOTE : la couleur de fond a été changée par sample.js, pour vérifier si le code js externe fonctionne)';
            },
            3000);
    });
