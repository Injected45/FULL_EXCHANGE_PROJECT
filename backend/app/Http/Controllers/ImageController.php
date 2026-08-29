<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Storage;
use App\Models\User;

class ImageController extends Controller
{
    // 🔹 رفع صورة واحدة
    public function uploadImage(Request $request)
    {
        $request->validate([
            'image' => 'required|image|mimes:jpg,jpeg,png,webp|max:4096',
        ]);

        $path = $request->file('image')->store('uploads/images', 'public');

        return response()->json([
            'status' => true,
            'path'   => $path,
            'url'    => asset('storage/' . $path),
        ]);
    }

    // 🔹 رفع عدة صور
    public function uploadMultiple(Request $request)
    {
        $request->validate([
            'images'   => 'required|array',
            'images.*' => 'image|mimes:jpg,jpeg,png,webp|max:4096',
        ]);

        $paths = [];

        foreach ($request->file('images') as $image) {
            $paths[] = $image->store('uploads/images', 'public');
        }

        return response()->json([
            'status' => true,
            'paths'  => $paths,
        ]);
    }

    // 🔹 تعديل (استبدال) صورة مستخدم
    public function updateImage(Request $request, $id)
    {
        $request->validate([
            'image' => 'required|image|mimes:jpg,jpeg,png,webp|max:4096',
        ]);

        $user = User::findOrFail($id);

        // حذف الصورة القديمة
        if ($user->image && Storage::disk('public')->exists($user->image)) {
            Storage::disk('public')->delete($user->image);
        }

        // رفع الصورة الجديدة
        $path = $request->file('image')->store('uploads/images', 'public');

        // تحديث المسار في DB
        $user->update([
            'image' => $path,
        ]);

        return response()->json([
            'status' => true,
            'path'   => $path,
            'url'    => asset('storage/' . $path),
        ]);
    }
}
