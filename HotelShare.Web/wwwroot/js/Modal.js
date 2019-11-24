var modal = new tingle.modal({
    footer: true,
    stickyFooter: false,
    closeMethods: ['overlay', 'button', 'escape'],
    closeLabel: "Close",
    cssClass: ['custom-class-1', 'custom-class-2'],
    onOpen: function () {

    },
    onClose: function () {

    },
    beforeClose: function () {
        // here's goes some logic
        // e.g. save content before closing the modal
        return true; // close the modal
        return false; // nothing happens
    }
});;


modal.init();

var btns = document.querySelectorAll(".trigger-button");

btns.forEach(function (elem) {
    elem.addEventListener('click', function () {
        var modalForm = document.querySelector('.tingle-modal-box__content');

        modalForm.childNodes.forEach(function (child) {
            modalForm.removeChild(child);
        });

        // Create link for delete
        var deleteLink = document.createElement('a');
        var message = document.createElement('p');
        message.innerHTML = "Are you sure you want to delete this comment?";
        deleteLink.classList += "delete-link-modal";
        deleteLink.href = this.getAttribute("deleteUrl");
        deleteLink.innerHTML = "Delete";

        // Create container for links
        var divLink = document.createElement('div');

        // Adding links to container
        divLink.appendChild(message);
        divLink.appendChild(deleteLink);

        // Adding container with links to modal form
        modalForm.appendChild(divLink);

        modal.setContent(modalForm.innerHTML);
        modal.open();
    });
});
