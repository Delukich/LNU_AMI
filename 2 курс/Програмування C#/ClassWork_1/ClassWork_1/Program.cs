using System;
using System.Collections.Generic;

class Contact
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public Contact(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public override string ToString()
    {
        return $"{FirstName}, {LastName}, {Email}";
    }
}

class ContactList
{
    private List<Contact> contacts = new List<Contact>();
    
    public delegate bool SearchContact(Contact contact);
    
    public event Action<Contact> ContactAdded;
    public event Action<Contact> ContactRemoved;
    public event Action<Contact> ContactUpdated;
    
    public void AddContact(Contact contact)
    {
        contacts.Add(contact);
        ContactAdded?.Invoke(contact);
    }
    
    public void RemoveContact(Contact contact)
    {
        if (contacts.Remove(contact))
        {
            ContactRemoved?.Invoke(contact);
        }
    }
    
    public void UpdateContact(Contact oldContact, Contact newContact)
    {
        int index = contacts.IndexOf(oldContact);

        if (index != -1)
        {
            contacts[index] = newContact;
            ContactUpdated?.Invoke(newContact);
        }
    }

    public List<Contact> FindContacts(SearchContact searchContact)
    {
        return contacts.FindAll(new Predicate<Contact>(searchContact));
    }

    public List<Contact> FindContactsWithEmail()
    {
        return contacts.FindAll(x => !string.IsNullOrEmpty(x.Email));
    }
    
    public void Display()
    {
        foreach (var item in contacts)
        {
            Console.WriteLine(item);
        }
    }
}

class Program
{
    static void Main()
    {
        ContactList contactList = new ContactList();
        
        contactList.ContactAdded += x => Console.WriteLine($"Contact Added: {x}");
        contactList.ContactRemoved += x => Console.WriteLine($"Contact Removed: {x}");
        contactList.ContactUpdated += x => Console.WriteLine($"Contact Updated: {x}");
        
        Contact contact1 = new Contact("Denys", "Lukianchuk", "denysluk100@gmail.com");
        Contact contact2 = new Contact("Petro", "Petrenko", "petropet12@gmail.com");
        Contact contact3 = new Contact("Ivan", "Petrenko", "");
        
        contactList.AddContact(contact1);
        contactList.AddContact(contact2);
        contactList.AddContact(contact3);
        
        Console.WriteLine("=============================================================");
        contactList.Display();
        Console.WriteLine("=============================================================");
        var foundContacts = contactList.FindContacts(x => x.FirstName == "Denys");
        foundContacts.ForEach(Console.WriteLine);
        Console.WriteLine("=============================================================");
        
        var foundContactsWithEmail = contactList.FindContactsWithEmail();
        foundContactsWithEmail.ForEach(Console.WriteLine);
        Console.WriteLine("=============================================================");
    }
}