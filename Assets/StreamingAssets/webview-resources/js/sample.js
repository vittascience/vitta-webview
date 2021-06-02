window.addEventListener(
    'load',
    function() {
        window.setTimeout(
            function() {
                document.body.style.backgroundColor = 'rgb(221, 255, 214)';
                var msg = document.getElementById("msg");
                msg.innerHTML += '<br>(NOTE : la couleur de fond a été changée par sample.js, pour vérifier si le code js externe fonctionne)';
            },
            3000);
    });
