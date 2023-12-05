
let cNumber = document.getElementById('number');
let num = document.getElementById('number').value;
cNumber.addEventListener('keyup', function (e) {
    
    let newValue = '';
    num = num.replace(/\s/g, '');
    for (var i = 0; i < num.length; i++) {
        if (i % 4 == 0 && i > 0) newValue = newValue.concat(' ');
        newValue = newValue.concat(num[i]);
        document.getElementById('number').value = newValue;
    }

    let ccNum = document.getElementById('number');
    if (document.getElementById('number').value.length == 16) {
        ccNum.style.border = "1px solid greenyellow";
        check();
    } else {
        ccNum.style.border = "1px solid red";
    }
});


let cvv = document.getElementById('cvv');
cvv.addEventListener('keyup', function (e) {

    let elen = cvv.value;
    let cvvBox = document.getElementById('cvv');
    if (elen.length == 3) {
        cvvBox.style.border = "1px solid greenyellow";
        check();
    } else {
        cvvBox.style.border = "1px solid red"; 
    }
})

var btn = document.getElementById("subbtn");
function check() {
    if (document.getElementById('number').value.length == 16 && document.getElementById('cvv').value.length == 3) {
        btn.removeAttribute('disabled');
    }
}




let popup = document.getElementById("popup");
function openPopup() {
    popup.classList.add("open-popup");
}

function closePopup() {
    popup.classList.remove("open-popup");
}
