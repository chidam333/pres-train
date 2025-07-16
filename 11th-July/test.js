document.addEventListener('DOMContentLoaded', function() {
    let ele = document.querySelector('body');
    console.log({ele})
    setInterval(() => {
        ele.innerHTML += 'hello world';
    }, 1000);
});