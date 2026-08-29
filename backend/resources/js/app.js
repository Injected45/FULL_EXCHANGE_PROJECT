import './bootstrap';
import Echo from "laravel-echo";

window.Pusher = require('pusher-js');

window.Echo = new Echo({
    broadcaster: 'pusher',
    key: 'your_app_key',
    cluster: 'your_cluster',
    encrypted: true
});

window.Echo.channel('notifications')
    .listen('notification.sent', (e) => {
        console.log('Received notification:', e.message);
        alert('وصل إشعار: ' + e.message);
    });

