<?php

namespace App\Services;

use Twilio\Rest\Client;

class TwilioService
{
    protected $twilio;

    public function __construct()
    {
        $this->twilio = new Client(
            config('services.twilio.sid'),
            config('services.twilio.token')
        );
    }

    public function sendSMS($to, $body)
    {
        $message = $this->twilio->messages->create($to, [
            'messagingServiceSid' => config('services.twilio.messaging_service_sid'),
            'body' => $body,
        ]);

        return $message->sid;
    }
}
