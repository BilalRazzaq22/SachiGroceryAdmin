using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROCERY.DAL.Core
{
    public class OfferRepo
    {
        GROCERYEntities gEnt = new GROCERYEntities();

        public List<OFFER_MANAGEMENT> GetOffers()
        {
            var q = from c in gEnt.OFFER_MANAGEMENT
                    select c;
            //if (q.Any())
            //{
            //    return q.ToList();
            //}
            return q.ToList();
        }

        public void AddOffer(OFFER_MANAGEMENT offer)
        {
            gEnt.OFFER_MANAGEMENT.Add(offer);
            gEnt.SaveChanges();
        }

        public OFFER_MANAGEMENT getOfferById(int offerId)
        {
            try
            {
                var q = from c in gEnt.OFFER_MANAGEMENT
                        where c.OFFER_ID == offerId
                        select c;
                if (q.Any())
                {
                    return q.FirstOrDefault();
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateOffer(OFFER_MANAGEMENT off)
        {
            try
            {
                OFFER_MANAGEMENT offer = getOfferById(off.OFFER_ID);
                if (offer != null)
                {
                    offer.USER_ID = off.USER_ID;
                    offer.CATEGORY_ID = off.CATEGORY_ID;
                    offer.SUB_CATEGORY_ID = off.SUB_CATEGORY_ID;
                    offer.PRODUCT_ID = off.PRODUCT_ID;
                    offer.CATEGORY_NAME = off.CATEGORY_NAME;
                    offer.SUB_CATEGORY_NAME = off.SUB_CATEGORY_NAME;
                    offer.PRODUCT_NAME = off.PRODUCT_NAME;
                    offer.TYPE = off.TYPE;
                    offer.DISCOUNT = off.DISCOUNT;
                    offer.UPDATED_ON = DateTime.Now;
                    offer.UPDATED_BY = off.UPDATED_BY;
                    gEnt.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void DeleteOffer(int id)
        {
            OFFER_MANAGEMENT offer = getOfferById(id);
            if (offer != null)
            {
                gEnt.OFFER_MANAGEMENT.Remove(offer);
                gEnt.SaveChanges();
            }
        }
    }
}