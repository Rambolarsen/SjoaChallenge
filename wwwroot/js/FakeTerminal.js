const scrollingElement = (document.scrollingElement || document.body);

function scrollToBottom() {
    scrollingElement.scrollTop = scrollingElement.scrollHeight;
}

function clearTerminal() {
   document.getElementById('output').innerHTML = "";    
};

window.appendToTerminal = (element, content) => {
    const pre = document.createElement("pre");
    pre.textContent = content;
    element.appendChild(pre);
};