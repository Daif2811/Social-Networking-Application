﻿using Forum.DAL;
using Forum.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.IRepository.Repository
{
    public class FollowRepository : IFollowRepository
    {
        private readonly ForumContext _context;

        public FollowRepository(ForumContext context)
        {
           _context = context;
        }

        public ICollection<Follow> GetAll()
        {
           return _context.Follows.Include(a => a.Follower).Include(a => a.Followed).ToList(); 
        }
       

        public Follow GetById(int id)
        {
           return _context.Follows.Where(a => a.Id == id).Include(a => a.Followed).Include(a => a.Follower).SingleOrDefault(); 
            
        }
        public Follow GetByUserId(string followedId , string followerId)
        {
           return _context.Follows.Where(a => a.FollowerId == followerId && a.FollowedId == followedId).Include(a => a.Followed).Include(a => a.Follower).SingleOrDefault(); 
            
        }
        public bool CheckFollow(string followedId , string followerId)
        {
            return _context.Follows.Any(a => a.FollowerId == followerId && a.FollowedId == followedId);
            
        }

        public ICollection<Follow> GetFollowerByUserId(string followedId)
        {
           return _context.Follows.Where(a => a.FollowedId == followedId).Include(a => a.Followed).Include(a => a.Follower).ToList(); 
        }
        public ICollection<Follow> GetFollowedByUserId(string followerId)
        {
           return _context.Follows.Where(a => a.FollowerId == followerId).Include(a => a.Followed).Include(a => a.Follower).ToList(); 
        }

        public async Task Add(Follow follow)
        {
           _context.Follows.Add(follow);
            await _context.SaveChangesAsync(); 
        }

        public async Task Update(Follow follow)
        {

            _context.Follows.Update(follow);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Follow follow = GetById(id);
            if (follow != null)
            {
                _context.Follows.Remove(follow);
                await _context.SaveChangesAsync();
            }
        }

    }



}
