$(document).ready(function () {
    $('.signup').magnificPopup({
        type: 'ajax'
    });
    var textDanger = $('.validation-summary-errors').text();
    if (textDanger != "") {
        $.notify({
            // options
            icon: 'fa fa-warning',
            title: '<strong>Warning</strong>: ',
            message: textDanger
        }, {
                type: 'warning',
                z_index: 1051,
                animate: {
                    enter: 'animated bounceIn',
                    exit: 'animated bounceOut'
                }
            });
    }
});

// notification on register
function Register(data) {
    if (data.data == "success") {
        $.notify({
            // options
            icon: 'fa fa-success',
            title: '<strong>Success</strong>: ',
            message: "Register is successfully, let's Sign In!"
        }, {
                type: 'success',
                z_index: 1051,
                animate: {
                    enter: 'animated bounceIn',
                    exit: 'animated bounceOut'
                }
            });
        $('.mfp-bg.mfp-ready').remove();
        $(".mfp-wrap").remove();
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

function ForgotPassword(data) {
    if (data.data == "success") {
        $.notify({
            // options
            icon: 'fa fa-check',
            title: '<strong>Success</strong>: ',
            message: "Please check your email, we send message. Click link in message to reset your password"
        }, {
                type: 'success',
                z_index: 1051,
                animate: {
                    enter: 'animated bounceIn',
                    exit: 'animated bounceOut'
                }
            });
        $('.mfp-bg.mfp-ready').remove();
        $(".mfp-wrap").remove();
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

(function ($) {
    "use strict";
   
    /*==================================================================
    [ Validate after type ]*/
    $('.validate-input .input100').each(function(){
        $(this).on('blur', function(){
            if(validate(this) == false){
                showValidate(this);
            }
            else {
                $(this).parent().addClass('true-validate');
            }
        })    
    })
  
  
    /*==================================================================
    [ Validate ]*/
    var input = $('.validate-input .input100');

    $('.validate-form').on('submit',function(){
        var check = true;

        for(var i=0; i<input.length; i++) {
            if(validate(input[i]) == false){
                showValidate(input[i]);
                check=false;
            }
        }

        return check;
    });


    $('.validate-form .input100').each(function(){
        $(this).focus(function(){
           hideValidate(this);
           $(this).parent().removeClass('true-validate');
        });
    });

     function validate (input) {
        if($(input).attr('type') == 'email' || $(input).attr('name') == 'email') {
            if($(input).val().trim().match(/^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{1,5}|[0-9]{1,3})(\]?)$/) == null) {
                return false;
            }
        }
        else {
            if($(input).val().trim() == ''){
                return false;
            }
        }
    }

    function showValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).addClass('alert-validate');

        $(thisAlert).append('<span class="btn-hide-validate">&#xf135;</span>')
        $('.btn-hide-validate').each(function(){
            $(this).on('click',function(){
               hideValidate(this);
            });
        });
    }

    function hideValidate(input) {
        var thisAlert = $(input).parent();
        $(thisAlert).removeClass('alert-validate');
        $(thisAlert).find('.btn-hide-validate').remove();
    }
    
    

})(jQuery);