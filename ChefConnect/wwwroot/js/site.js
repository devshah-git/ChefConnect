
// Load the carousel when the document is ready
$(document).ready(function () {
    $('.carousel').carousel();
});

document.addEventListener('DOMContentLoaded', (event) => {
    let images = [
        'images/foodImages/palak paneer.jpg',
        'images/foodImages/chole.jpg',
        'images/foodImages/vadapav.jpg',
        'images/foodImages/dabeli.jpg',
        'images/foodImages/friedchicken.jpg',
    ];
    let currentIndex = 0;
    setInterval(() => {
        // Update the src attribute of the image
        document.getElementById('highligh-image').src = images[currentIndex];
        // Increment currentIndex, reset if it exceeds array length
        currentIndex = (currentIndex + 1) % images.length;
    }, 3000); // Change every 3000 milliseconds (3 seconds)
})

function selectPaymentCard(cardElement) {
    // Remove 'selected-card' class from all cards
    document.querySelectorAll('.card-payment-home').forEach(function (card) {
        card.classList.remove('selected-card');
    });

    // Add 'selected-card' class to the clicked card
    cardElement.classList.add('selected-card');

    // Find the radio button inside the clicked card and check it
    var radio = cardElement.querySelector('.card-radio');
    if (radio) {
        radio.checked = true;
    }
}
function selectAddressCard(cardElement) {
    // Remove 'selected-card' class from all cards
    document.querySelectorAll('.card-address-home').forEach(function (card) {
        card.classList.remove('selected-card');
    });

    // Add 'selected-card' class to the clicked card
    cardElement.classList.add('selected-card');

    // Find the radio button inside the clicked card and check it
    var radio = cardElement.querySelector('.card-radio');
    if (radio) {
        radio.checked = true;
    }
}

//Load the animated text when the document is ready
document.addEventListener("DOMContentLoaded", function () {
    // Get the elements
    var animatedText = document.getElementById("animatedText");
    var animatedSubText = document.getElementById("animatedSubText");

    // Set initial styles
    animatedText.style.opacity = 0;
    animatedSubText.style.opacity = 0;
    animatedText.style.transform = "translateY(20px)";
    animatedSubText.style.transform = "translateY(20px)";

    // Add animation styles
    animatedText.style.transition = "opacity 1s ease, transform 1s ease";
    animatedSubText.style.transition = "opacity 1s ease, transform 1s ease";

    // Trigger the animation
    setTimeout(function () {
        animatedText.style.opacity = 1;
        animatedText.style.transform = "translateY(0)";
    }, 100);

    setTimeout(function () {
        animatedSubText.style.opacity = 1;
        animatedSubText.style.transform = "translateY(0)";
    }, 300);
});
window.addEventListener('load', function () {
    // Wait for the header animation to complete
    setTimeout(function () {
        document.getElementById('imageCarousel').classList.add('carousel-visible');
    }, 3000);

    // Delay function for the animation.
    $(document).ready(function () {
        $('.highlights-list li').each(function (i) {
            // Increase the delay by 0.2 seconds for each item
            $(this).css('animation-delay', `${0.2 * i}s`);
        });
    });

    // Animate the image in from the right
    $(document).ready(function () {
        var $image = $('.highlists-image');
        // Initially, set the image off-screen to the right
        $image.css({
            opacity: 0,
            transform: 'translateX(100%)'
        });

        // Function to trigger the animation
        function animateImage() {
            $image.css('opacity', 1);
            $image.addClass('animate-in-from-right');
        }

        setTimeout(animateImage, 3000);
    });

});

    document.addEventListener("DOMContentLoaded", function () {
        const colors = ["#FF6347", "#4682B4", "#32CD32", "#FFD700", "#6A5ACD", "#FF69B4"];
        const listItems = document.querySelectorAll('.highlights-list li');

        listItems.forEach((item) => {
            // Continuously change color every second
            setInterval(() => {
                if (!item.classList.contains('hover')) {
                    const randomColorIndex = Math.floor(Math.random() * colors.length);
                    item.style.color = colors[randomColorIndex];
                }
            }, 1000);

            item.addEventListener('mouseover', () => {
                item.classList.add('hover');
                const randomColorIndex = Math.floor(Math.random() * colors.length);
                item.style.color = colors[randomColorIndex];
            });

            item.addEventListener('mouseout', () => {
                item.classList.remove('hover');
            });
        });
    });




    document.addEventListener("DOMContentLoaded", function () {

        window.addEventListener('scroll', function () {
            const navbar = document.querySelector('body');
            if (window.scrollY > 50) {
                document.querySelector('.navbar').classList.add('scrolled');
                navbar.classList.add('navbar-scroll');
            } else {
                document.querySelector('.navbar').classList.remove('scrolled');
                navbar.classList.remove('navbar-scroll');
            }
            console.log("I was Called");
        });
    });


document.addEventListener("DOMContentLoaded", function () {
    // Select all navigation links
    var navLinks = document.querySelectorAll('.decorate-nav a');

    // Function to remove existing 'active' classes
    function removeActiveClasses() {
        navLinks.forEach(function (link) {
            link.classList.remove('active');
        });
    }

    navLinks.forEach(function (link) {
        link.addEventListener('click', function () {
            // Remove 'active' from all links on click
            removeActiveClasses();
            // Add 'active' class to the clicked link
            this.classList.add('active');
        });
    });
});










