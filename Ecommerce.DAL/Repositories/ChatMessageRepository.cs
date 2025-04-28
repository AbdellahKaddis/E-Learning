using Ecommerce.DAL.Db;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Repositories
{
    public class ChatMessageRepository
    {
        private readonly AppDbContext _context;

        public ChatMessageRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddMessageAsync(CreateChatMessageDTO message)
        {
            var chatMessage = new ChatMessage
            {
                StudentId = message.StudentId,
                InstructorId = message.InstructorId,
                SenderType = message.SenderType,
                MessageText = message.MessageText,
                SentAt = message.SentAt
            };

            await _context.ChatMessages.AddAsync(chatMessage);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ChatMessageDTO>> GetMessagesAsync(int studentId, int instructorId)
        {
            return await _context.ChatMessages
                .Where(m => (m.StudentId == studentId && m.InstructorId == instructorId) || (m.StudentId == instructorId && m.InstructorId == studentId))
                .OrderBy(m => m.SentAt)
                .Select(m => new ChatMessageDTO
                {
                    Id = m.Id,
                    StudentId = m.StudentId,
                    InstructorId = m.InstructorId,
                    SenderType = m.SenderType,
                    MessageText = m.MessageText,
                    SentAt = m.SentAt
                })
                .ToListAsync();
        }
    }
}
