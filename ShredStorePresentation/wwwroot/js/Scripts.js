function myFunction() {

    var e = document.getElementById('CategoryDropDown');
    var category = e.options[e.selectedIndex].text;
    console.log(category);

    removeOptions(document.getElementById('TypesDropDown'));

    if (category === "Eletric Guitar") {
        var types = ["6 Strings", "7 Strings", "8 Strings", "12 Strings"]
        types.forEach(InsertOption)
    }
    if (category === "Pedals") {
        var types = ["Distortion", "Fuzz", "Delay", "Flanger", "Wah"]
        types.forEach(InsertOption)
    }
    if (category === "Amplifier") {
        var types = ["Valvulated", "Bass", "Guitar", "PA"]
        types.forEach(InsertOption)
    }
    if (category === "Accessories") {
        var types = ["Strings", "Case", "Strap", "Pick", "Bag", "Cleaner"]
        types.forEach(InsertOption)
    }
    if (category === "Acoustic Guitar") {
        var types = ["6 Strings", "7 Strings", "8 Strings", "12 Strings"]
        types.forEach(InsertOption)
    }

}
function InsertOption(option) {
    select = document.getElementById('TypesDropDown');
    var opt = document.createElement('option');
    opt.value = option;
    opt.innerHTML = option;
    select.appendChild(opt);
}

function removeOptions(selectElement) {
    var i, L = selectElement.options.length - 1;
    for (i = L; i >= 0; i--) {
        selectElement.remove(i);
    }
}