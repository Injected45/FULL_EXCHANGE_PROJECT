<?php

return [

 

    'postmark' => [
        'token' => env('POSTMARK_TOKEN'),
    ],

    'ses' => [
        'key' => env('AWS_ACCESS_KEY_ID'),
        'secret' => env('AWS_SECRET_ACCESS_KEY'),
        'region' => env('AWS_DEFAULT_REGION', 'us-east-1'),
    ],

    'resend' => [
        'key' => env('RESEND_KEY'),
    ],

    'slack' => [
        'notifications' => [
            'bot_user_oauth_token' => env('SLACK_BOT_USER_OAUTH_TOKEN'),
            'channel' => env('SLACK_BOT_USER_DEFAULT_CHANNEL'),
        ],
    ],

    'twilio' => [
        'sid' => env('TWILIO_SID'),
        'token' => env('TWILIO_TOKEN'),
        'messaging_service_sid' => env('TWILIO_MESSAGING_SERVICE_SID'),
        'verify_sid' => env('TWILIO_VERIFY_SID'), // هذا هو المطلوب
    ],
    
    'whatsapp' => [
        'token' => env('WHATSAPP_TOKEN'),

        /*
        | وجهة تطوير لرموز التحقّق.
        |
        | حين يُضبط OTP_DEV_TO، يُرسَل كل رمز تحقّق إلى هذا الرقم بدل رقم
        | المستخدم، ويُتخطّى فحص وجود الرقم الليبي على واتساب. الهوية تبقى
        | الرقم الليبي المُدخَل — يتغيّر المُستلِم فقط.
        |
        | ⚠️ هذا المفتاح يجب ألّا يوجد في .env الإنتاج. غيابه يعني تعطّله،
        | فلا يعتمد على APP_ENV — الذي هو 'local' على الإنتاج أيضاً.
        */
        'dev_otp_to' => env('OTP_DEV_TO'),
    ],
];
