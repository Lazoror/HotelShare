var paggingBtn = document.querySelectorAll(".pagging-btn");
var filterForm = document.getElementById('filterForm');
var paggingForm = document.getElementById('pagination-items');

paggingBtn.forEach(function (elem) {
    elem.addEventListener("click", function () {
        document.getElementById("currentPage").setAttribute("value", this.value);
        filterForm.submit();
    });
});

paggingForm.addEventListener("change", function () {
    this.submit();
});