function ResetPassword(data) {
    console.log(data);
    if (data.data == "userNotFound") {
        $.notify({
            // options
            icon: 'fa fa-warning',
            title: '<strong>Warning</strong>: ',
            message: "User not found, please check and enter correct Email"
        }, {
                type: 'warning',
                z_index: 1051,
                animate: {
                    enter: 'animated bounceIn',
                    exit: 'animated bounceOut'
                }
            });
        return;
    }
    if (data.data == "success") {
        $.notify({
            // options
            icon: 'fa fa-success',
            title: '<strong>Success</strong>: ',
            message: "Password successfully changed, you will be redirected to login page!"
        }, {
                type: 'success',
                z_index: 1051,
                animate: {
                    enter: 'animated bounceIn',
                    exit: 'animated bounceOut'
                }
            });
        return setTimeout(function () { window.location.href = "/Account/Login"; }, 5000);
    }
    if (data.errorMessage.length >= 1) {
        data.errorMessage.forEach(function (item) {
            $.notify({
                // options
                icon: 'fa fa-warning',
                title: '<strong>Warning</strong>: ',
                message: item
            }, {
                    type: 'warning',
                    z_index: 1051,
                    animate: {
                        enter: 'animated bounceIn',
                        exit: 'animated bounceOut'
                    }
                });
        });
    }
    return;
}

function ChangePassword(data) {
    if (data.data == "success") {
        $.notify({
            // options
            icon: 'fa fa-success',
            title: '<strong>Success</strong>: ',
            message: "Password successfully changed, you will be redirected to login page!"
        }, {
                type: 'success',
                z_index: 1051,
                animate: {
                    enter: 'animated bounceIn',
                    exit: 'animated bounceOut'
                }
            });
        return setTimeout(function () { window.location.href = "/Account/Login"; }, 3000);
    }
    if (data.errorMessage.length >= 1) {
        data.errorMessage.forEach(function (item) {
            $.notify({
                // options
                icon: 'fa fa-warning',
                title: '<strong>Warning</strong>: ',
                message: item
            }, {
                    type: 'warning',
                    z_index: 1051,
                    animate: {
                        enter: 'animated bounceIn',
                        exit: 'animated bounceOut'
                    }
                });
        });
    }
    return;
}

$("#btnCreatePlayList").on("click", function () {

    var url = $(this).data("url");

    $.get(url, function (data) {
        $('#createPlayListContainer').html(data);

        $('#createPlayListModal').modal('show');
        
        $('.chosen-select').chosen({
            no_results_text: "Oops, nothing found!",
            placeholder_text_multiple: "Please, select some option",
            hide_results_on_select: false
        }).on('change', function (obj, result) {
            console.debug("changed: %o", arguments);
        });
    });

});
$(".artist").on("click", "#btnCreatePlayList", function () {

    var url = $(this).data("url");

    $.get(url, function (data) {
        $('#createPlayListContainer').html(data);

        $('#createPlayListModal').modal('show');

        $('.chosen-select').chosen({
            no_results_text: "Oops, nothing found!",
            placeholder_text_multiple: "Please, select some option",
            hide_results_on_select: false
        }).on('change', function (obj, result) {
            console.debug("changed: %o", arguments);
        });
    });

});
$(".artist").on("click", "#btnDeletePlaylist", function () {

    var url = $(this).data("url");

    $.get(url, function (data) {
        $('#deletePlayListContainer').html(data);

        $('#deletePlayListModal').modal('show');
    });

});

function DeletePlaylistSuccess(data) {

    if (data.data != "success") {
        $('#deletePlayListContainer').html(data.data);
        return;
    }
    $.notify({
        // options
        icon: 'fa fa-check-circle',
        title: '<strong>Success</strong>: ',
        message: 'Deleted!'
    }, {
            type: 'success',
            z_index: 1051,
            animate: {
                enter: 'animated bounceIn',
                exit: 'animated bounceOut'
            }
        });
    $('#deletePlayListModal').modal('hide');
    $('#deletePlayListContainer').html("");
    window.location.reload();  
}

$(function Rating() {

    var $rateYo = $("#rateYo").rateYo();

    $("#getRating").click(function () {

        /* get rating */
        var rating = $rateYo.rateYo("rating");

        window.alert("Its " + rating + " Yo!");
    });


});
function rating() {
    var slider = document.getElementById("myRange");
    var output = document.getElementById("demo");
    output.innerHTML = slider.value;
    var id = $('#modelId').val();
    slider.oninput = function () {
        output.innerHTML = this.value;
    };
    $.ajax({
        url: "/Playlists/RatePlaylist/" + id,
        type: "POST",
        data: {
            "ratingFromUser": slider.value
        }
    });
}

function AddComment(data) {
    if (data.data == "success") {
        $('#small-dialog').load("/Playlists/Details/" + data.id);
        $.notify({
            // options
            icon: 'fa fa-success',
            title: '<strong>Success</strong>: ',
            message: "Comment added!"
        }, {
                type: 'success',
                z_index: 1051,
                animate: {
                    enter: 'animated bounceIn',
                    exit: 'animated bounceOut'
                }
            });
        return;
    }
    if (data.errorMessage.length >= 1) {
        data.errorMessage.forEach(function (item) {
            $.notify({
                // options
                icon: 'fa fa-warning',
                title: '<strong>Warning</strong>: ',
                message: item
            }, {
                    type: 'warning',
                    z_index: 1051,
                    animate: {
                        enter: 'animated bounceIn',
                        exit: 'animated bounceOut'
                    }
                });
        });
    }
    return;
}

function CreatePlaylistSuccess(data) {
    if (data.data != "success") {
        $('#createSongContainer').html(data.data);
        return;
    }
    $('#createPlayListModal').modal('hide');
    $('#createPlayListContainer').html("");
    $.notify({
        // options
        icon: 'fa fa-success',
        title: '<strong>Success</strong>: ',
        message: "Playlist was successfully added!"
    }, {
            type: 'success',
            z_index: 1051,
            animate: {
                enter: 'animated bounceIn',
                exit: 'animated bounceOut'
            }
        });
    window.location.reload();  
}
$(document).ready(function () {
    "use strict";
    
    //------- Lightbox  js --------//  

    $('.img-pop-up').magnificPopup({
        type: 'ajax'
    });

    $('.see-img').magnificPopup({
        type: 'image',
        gallery: {
            enabled: true
        }
    });

    $('.play-btn').magnificPopup({
        type: 'iframe',
        mainClass: 'mfp-fade',
        removalDelay: 160,
        preloader: false,
        fixedContentPos: false
    });

    //------- Counter  js --------//  

     if(document.getElementById("facts-area")){
      $('.counter').counterUp({
          delay: 10,
          time: 1000
      });
    }

    //------- Skill  js --------//  

    $('.skill').simpleSkillbar();

    //------- Filter  js --------//  

      $('.filters ul li').click(function(){
        $('.filters ul li').removeClass('active');
        $(this).addClass('active');
        
        var data = $(this).attr('data-filter');
        $grid.isotope({
          filter: data
        })
      });


      if(document.getElementById("portfolio")){
            var $grid = $(".grid").isotope({
              itemSelector: ".all",
              percentPosition: true,
              masonry: {
                columnWidth: ".all"
              }
            })
      };


    //------- Timeline js --------//  


    $('.content').each( function(i){
      
      var bottom_of_object= $(this).offset().top + $(this).outerHeight();
      var bottom_of_window = $(window).height();
      
      if( bottom_of_object > bottom_of_window){
        $(this).addClass('hidden');
      }
    });


    $(window).scroll( function(){
        /* Check the location of each element hidden */
        $('.hidden').each( function(i){
          
            var bottom_of_object = $(this).offset().top + $(this).outerHeight();
            var bottom_of_window = $(window).scrollTop() + $(window).height();
          
            /* If the object is completely visible in the window, fadeIn it */
            if( bottom_of_window > bottom_of_object ){
              $(this).animate({'opacity':'1'},700);
            }
        });
    });


    //------- Superfish nav menu  js --------//  

    $('.nav-menu').superfish({
        animation: {
            opacity: 'show'
        },
        speed: 400
    });

    //------- Accordian Js --------//  

    //var allPanels = $(".accordion > dd").hide();
    //allPanels.first().slideDown("easeOutExpo");
    //$(".accordion").each(function() {
    //    $(this).find("dt > a").first().addClass("active").parent().next().css({
    //        display: "block"
    //    });
    //});


    // $(document).on('click', '.accordion > dt > a', function(e) {

    //    var current = $(this).parent().next("dd");
    //    $(this).parents(".accordion").find("dt > a").removeClass("active");
    //    $(this).addClass("active");
    //    $(this).parents(".accordion").find("dd").slideUp("easeInExpo");
    //    $(this).parent().next().slideDown("easeOutExpo");

    //    return false;

    //});

    //------- Tabs Js --------//  
    if (document.getElementById("horizontalTab")) {

    $('#horizontalTab').jqTabs({
        direction: 'horizontal',
        duration: 200
    });
    
    };  


    //------- Owl Carusel  js --------//  

    $('.active-review-carusel').owlCarousel({
        items:1,
        loop:true,
        autoplay:true,
        autoplayHoverPause: true,        
        margin:30,
        dots: true
    });

     $('.active-testimonial').owlCarousel({
            items: 2,
            loop: true,
            margin: 30,
            autoplayHoverPause: true,
            dots: true,
            autoplay: true,
            nav: true,
            navText: ["<span class='lnr lnr-arrow-up'></span>", "<span class='lnr lnr-arrow-down'></span>"],
            responsive: {
                0: {
                    items: 1
                },
                480: {
                    items: 1,
                },
                768: {
                    items: 2,
                }
            }
        });



    $('.active-brand-carusel').owlCarousel({
        items: 5,
        loop: true,
        autoplayHoverPause: true,
        autoplay: true,
        responsive: {
            0: {
                items: 1
            },
            455: {
                items: 2
            },            
            768: {
                items: 3,
            },
            991: {
                items: 4,
            },
            1024: {
                items: 5,
            }
        }
    }); 

    //------- Mobile Nav  js --------//  

    if ($('#nav-menu-container').length) {
        var $mobile_nav = $('#nav-menu-container').clone().prop({
            id: 'mobile-nav'
        });
        $mobile_nav.find('> ul').attr({
            'class': '',
            'id': ''
        });
        $('body').append($mobile_nav);
        $('body').prepend('<button type="button" id="mobile-nav-toggle"><i class="lnr lnr-menu"></i></button>');
        $('body').append('<div id="mobile-body-overly"></div>');
        $('#mobile-nav').find('.menu-has-children').prepend('<i class="lnr lnr-chevron-down"></i>');

        $(document).on('click', '.menu-has-children i', function(e) {
            $(this).next().toggleClass('menu-item-active');
            $(this).nextAll('ul').eq(0).slideToggle();
            $(this).toggleClass("lnr-chevron-up lnr-chevron-down");
        });

        $(document).on('click', '#mobile-nav-toggle', function(e) {
            $('body').toggleClass('mobile-nav-active');
            $('#mobile-nav-toggle i').toggleClass('lnr-cross lnr-menu');
            $('#mobile-body-overly').toggle();
        });

            $(document).on('click', function(e) {
            var container = $("#mobile-nav, #mobile-nav-toggle");
            if (!container.is(e.target) && container.has(e.target).length === 0) {
                if ($('body').hasClass('mobile-nav-active')) {
                    $('body').removeClass('mobile-nav-active');
                    $('#mobile-nav-toggle i').toggleClass('lnr-cross lnr-menu');
                    $('#mobile-body-overly').fadeOut();
                }
            }
        });
    } else if ($("#mobile-nav, #mobile-nav-toggle").length) {
        $("#mobile-nav, #mobile-nav-toggle").hide();
    }

    //------- Smooth Scroll  js --------//  

    $('.nav-menu a, #mobile-nav a, .scrollto').on('click', function() {
        if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
            var target = $(this.hash);
            if (target.length) {
                var top_space = 0;

                if ($('#header').length) {
                    top_space = $('#header').outerHeight();

                    if (!$('#header').hasClass('header-fixed')) {
                        top_space = top_space;
                    }
                }

                $('html, body').animate({
                    scrollTop: target.offset().top - top_space
                }, 1500, 'easeInOutExpo');

                if ($(this).parents('.nav-menu').length) {
                    $('.nav-menu .menu-active').removeClass('menu-active');
                    $(this).closest('li').addClass('menu-active');
                }

                if ($('body').hasClass('mobile-nav-active')) {
                    $('body').removeClass('mobile-nav-active');
                    $('#mobile-nav-toggle i').toggleClass('lnr-times lnr-bars');
                    $('#mobile-body-overly').fadeOut();
                }
                return false;
            }
        }
    });

    $(document).ready(function() {

        $('html, body').hide();

        if (window.location.hash) {

            setTimeout(function() {

                $('html, body').scrollTop(0).show();

                $('html, body').animate({

                    scrollTop: $(window.location.hash).offset().top - 108

                }, 1000)

            }, 0);

        } else {

            $('html, body').show();

        }

    });


    jQuery(document).ready(function($) {
        // Get current path and find target link
        var path = window.location.pathname.split("/").pop();

        // Account for home page with empty path
        if (path == '') {
            path = 'index.html';
        }

        var target = $('nav a[href="' + path + '"]');
        // Add active class to target link
        target.addClass('menu-active');
    });

    $(document).ready(function() {
        if ($('.menu-has-children ul>li a').hasClass('menu-active')) {
            $('.menu-active').closest("ul").parentsUntil("a").addClass('parent-active');
        }
    });




    //------- Header Scroll Class  js --------//  

    $(window).scroll(function() {
        if ($(this).scrollTop() > 100) {
            $('#header').addClass('header-scrolled');
        } else {
            $('#header').removeClass('header-scrolled');
        }
    });

    

    //------- Mailchimp js --------//  

    $(document).ready(function() {
        $('#mc_embed_signup').find('form').ajaxChimp();
    });

});