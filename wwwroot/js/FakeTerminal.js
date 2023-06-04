const scrollingElement = (document.scrollingElement || document.body);

function scrollToBottom() {
    scrollingElement.scrollTop = scrollingElement.scrollHeight;
}

function getUserAgent() {
    return navigator.userAgent;
};