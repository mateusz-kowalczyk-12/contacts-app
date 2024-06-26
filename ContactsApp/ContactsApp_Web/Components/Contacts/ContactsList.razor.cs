﻿using Microsoft.AspNetCore.Components;
using ContactsApp_Models.Responses;
using ContactsApp_Web.States;

namespace ContactsApp_Web.Components.Contacts;

public partial class ContactsList
{
    [Inject] private AppState AppState { get; set; } = null!;
}