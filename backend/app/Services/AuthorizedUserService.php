<?php

namespace App\Services;

use App\Models\AuthorizedUser;

class AuthorizedUserService
{
    // إضافة
    public function create($data)
    {
        return AuthorizedUser::create([
            'Name_post'         => $data['Name_post'],
            'BranchID'     => $data['BranchID'],
            'UserID'       => $data['UserID'],
            'InsertUserID' => $data['InsertUserID'] ?? null,
            'AccID'        => $data['AccID'] ?? null,
            'IsActive'     => $data['IsActive'] ?? 1,
            'CreatedDate'  => now() ,
            'phone'        =>$data['phone']
        ]);
    }

    // تعديل
    public function update($id, $data)
{
    $user = AuthorizedUser::findOrFail($id);

    $user->update([
        'Name_post' => $data['Name'] ?? $user->Name_post,
        'BranchID'  => $data['BranchID'] ?? $user->BranchID,
        'UserID'    => $data['UserID'] ?? $user->UserID,
        'AccID'     => $data['AccID'] ?? $user->AccID,
        'IsActive'  => $data['IsActive'] ?? $user->IsActive,
        'phone'     => $data['phone'] ?? $user->phone,
    ]);

    return $user;
}

    // جلب حسب الفرع
    public function getByBranch($branchId)
    {
        return AuthorizedUser::where('BranchID', $branchId)
            
            ->orderBy('ID', 'desc')
            ->get();
    }
}