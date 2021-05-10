//open and close tab menu
$('.nav-tabs-dropdown')
    .on("click", "li:not('.active') a", function(event) {  $(this).closest('ul').removeClass("open");
    })
    .on("click", "li.active a", function(event) {        $(this).closest('ul').toggleClass("open");
    });

$('.owl-carousel').owlCarousel({
    loop:true,
    margin:15,
    responsiveClass:true,
	nav:true,	
	navText: ["<i class='fa fa-long-arrow-left'></i>","<i class='fa fa-long-arrow-right'></i>"],
    responsive:{
        0:{
            items:1,
        },
        600:{
            items:2
        },
        1000:{
            items:3,
            loop:false
        }
    }
}); 






  
