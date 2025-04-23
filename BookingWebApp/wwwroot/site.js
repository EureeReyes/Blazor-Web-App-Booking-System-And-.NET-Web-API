document.addEventListener("DOMContentLoaded", function () {
    document.querySelector(".bb-sidebar-content")?.classList.remove("collapse");
});

window.removeSidebarCollapse = () => {
    document.querySelector(".bb-sidebar-content")?.classList.remove("collapse");
};

window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement("a");
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
};

window.bookingModal = {
    addShowClass: function (element) {
        if (!element) return;
        element.classList.add('show');  // Add 'show' class to trigger height expansion
    }
};



