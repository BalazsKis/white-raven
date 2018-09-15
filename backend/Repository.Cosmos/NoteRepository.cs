using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Repository.Contract;
using WhiteRaven.Repository.Contract.Exceptions;
using WhiteRaven.Repository.Cosmos.Entities;

namespace WhiteRaven.Repository.Cosmos
{
    public sealed class NoteRepository : RepositoryBase<Note>, INoteRepository
    {
        private const string CollectionName = "Notes";

        public NoteRepository(IKeyFor<Note> keyProvider) : base(CollectionName, keyProvider)
        {
        }

        public async Task<IEnumerable<Note>> GetByNoteIds(IEnumerable<string> noteIds)
        {
            try
            {
                var ids = noteIds.ToArray();

                var response = await Client
                    .CreateDocumentQuery<StoredEntity<Note>>(DocumentCollectionUri)
                    .Where(note => ids.Contains(note.Id))
                    .AsDocumentQuery()
                    .ExecuteNextAsync<StoredEntity<Note>>();

                return response.Select(e => e.Entity).ToList();
            }
            catch (Exception ex)
            {
                throw new ReadFailedException(typeof(Note), ex);
            }
        }
    }
}