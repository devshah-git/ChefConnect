import axios from 'https://cdn.jsdelivr.net/npm/axios@1.3.5/+esm';
import FormData from 'https://cdn.jsdelivr.net/npm/form-data@4.0.0/+esm';

const chatbotToggler = document.querySelector(".chatbot-toggler");
const closeBtn = document.querySelector(".close-btn");
const chatbox = document.querySelector(".chatbox");
const chatInput = document.querySelector(".chat-input textarea");
const sendChatBtn = document.querySelector(".chat-input span");

let userMessage = null; // Variable to store user's message
const inputInitHeight = chatInput.scrollHeight;

const createChatLi = (message, className) => {
    // Create a chat <li> element with passed message and className
    const chatLi = document.createElement("li");
    chatLi.classList.add("chat", `${className}`);
    let chatContent = className === "outgoing" ? `<p></p>` : `<span class="material-symbols-outlined">smart_toy</span><p></p>`;
    chatLi.innerHTML = chatContent;
    chatLi.querySelector("p").textContent = message;
    return chatLi; // return chat <li> element
}

const generateResponse = (chatElement) => {
    // const API_URL = "https://api.sightengine.com/1.0/text/check.json";
    const messageElement = chatElement.querySelector("p");
    // Define the properties and message for the API request
    let data = new FormData();
    data.append('text', userMessage);
    data.append('lang', 'en');
    data.append('mode', 'rules');
    data.append('api_user', '1845854402');
    data.append('api_secret', 'L2WiDExKHPSxUU6k46r3mEdakuNTTjuT');

    axios({
        url: 'https://api.sightengine.com/1.0/text/check.json',
        method: 'post',
        data: data,
        headers: data.getHeaders
        
    })
        .then(function (response) {
            // on success: handle response
            console.log(response.data);
            messageElement.textContent = "";
            let matchesCount = response.data.profanity.matches.length
            for (let i = 0; i < matchesCount; i++) {
                messageElement.textContent += "Type: " + response.data.profanity.matches[i].type +
                    "\nIntensity: " + response.data.profanity.matches[i].intensity +
                    "\nContent: " + response.data.profanity.matches[i].match + "\n\n";
            }
           
        })
        .catch(function (error) {
            // handle error
            if (error.response) console.log(error.response.data);
            else console.log(error.message);
        }).finally(() => chatbox.scrollTo(0, chatbox.scrollHeight));

}

const handleChat = () => {
    userMessage = chatInput.value.trim(); // Get user entered message and remove extra whitespace
    if (!userMessage) return;

    // Clear the input textarea and set its height to default
    chatInput.value = "";
    chatInput.style.height = `${inputInitHeight}px`;

    // Append the user's message to the chatbox
    chatbox.appendChild(createChatLi(userMessage, "outgoing"));
    chatbox.scrollTo(0, chatbox.scrollHeight);

    setTimeout(() => {
        // Display "Thinking..." message while waiting for the response
        const incomingChatLi = createChatLi("Thinking...", "incoming");
        chatbox.appendChild(incomingChatLi);
        chatbox.scrollTo(0, chatbox.scrollHeight);
        generateResponse(incomingChatLi);
    }, 600);
}

const assignEventListeners = () => {

    chatInput.addEventListener("input", () => {
        // Adjust the height of the input textarea based on its content
        chatInput.style.height = `${inputInitHeight}px`;
        chatInput.style.height = `${chatInput.scrollHeight}px`;
    });

    chatInput.addEventListener("keydown", (e) => {
        // If Enter key is pressed without Shift key and the window 
        // width is greater than 800px, handle the chat
        if (e.key === "Enter" && !e.shiftKey && window.innerWidth > 800) {
            e.preventDefault();
            handleChat();
        }
    });

    sendChatBtn.addEventListener("click", handleChat);
    closeBtn.addEventListener("click", () => document.body.classList.remove("show-chatbot"));
    chatbotToggler.addEventListener("click", () => document.body.classList.toggle("show-chatbot"));
};

$(document).ready(() => {

    assignEventListeners();

});